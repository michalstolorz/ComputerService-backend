using AutoMapper;
using ComputerService.Core.Dto.Request;
using ComputerService.Core.Dto.Response;
using ComputerService.Core.Exceptions;
using ComputerService.Core.Helpers;
using ComputerService.Core.Validators;
using ComputerService.Data.Models;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;


namespace ComputerService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IUserContextProvider _userContextProvider;

        public AuthenticationController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager,
            IMapper mapper, IConfiguration configuration, IDateTimeProvider dateTimeProvider, IUserContextProvider userContextProvider)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _configuration = configuration;
            _dateTimeProvider = dateTimeProvider;
            _userContextProvider = userContextProvider;
        }

        /// <summary>
        /// Generates and returns JWT Token for authorization.
        /// </summary>
        /// <param name="request">Request with credentials email and password</param>
        /// <param name="cancellationToken">Propagates notification that operation should be canceled</param>
        /// <returns>A newly created JWT token</returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Login(LoginRequest request, CancellationToken cancellationToken)
        {
            var validator = new LoginRequestValidator();
            await validator.ValidateAndThrowAsync(request, null, cancellationToken);

            var _privateKey = _configuration.GetSection("AppSettings:PrivateKey").Value;
            if (String.IsNullOrEmpty(_privateKey))
            {
                throw new ControllerException(ErrorCodes.PrivateKeyNotFound, "Controller couldn't retrieve private key");
            }
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
                if (result.Succeeded)
                {
                    var key = TokenHelper.BuildRsaSigningKey(_privateKey);
                    var userRoles = await _userManager.GetRolesAsync(user);
                    var token = TokenHelper.GenerateToken(user.Id, userRoles, key, _dateTimeProvider);
                    var loggedUser = _mapper.Map<LoginResponse>(user);
                    loggedUser.Token = token;

                    return Ok(loggedUser);
                }
            }

            return Unauthorized();
        }

        /// <summary>
        /// Logout
        /// </summary>
        /// <returns></returns>
        [HttpPost("logout")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return Ok();
        }

        /// <summary>
        /// Change password
        /// </summary>
        /// <param name="request">Request with old password, new password and new password confirmation</param>
        /// <param name="cancellationToken">Propagates notification that operation should be canceled</param>
        /// <returns></returns>
        [HttpPost("changePassword")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<IdentityError>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request, CancellationToken cancellationToken)
        {
            var validator = new ChangePasswordRequestValidator();
            await validator.ValidateAndThrowAsync(request, null, cancellationToken);

            var user = await _userManager.GetUserAsync(_userContextProvider.User);

            if (user != null)
            {
                var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);

                if (result.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            else
            {
                return Unauthorized();
            }
        }

        /// <summary>
        /// Reset password
        /// </summary>
        /// <param name="userId">User id for which to reset the password</param>
        /// <param name="cancellationToken">Propagates notification that operation should be canceled</param>
        /// <returns>Identity result</returns>
        [HttpPost("resetPassword/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ResetPassword(int userId, CancellationToken cancellationToken)
        {
            var validator = new IdValidator();
            await validator.ValidateAndThrowAsync(userId, null, cancellationToken);

            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                return Unauthorized();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, "mojeNaprawy2020");

            return Ok(result);
        }
    }
}
