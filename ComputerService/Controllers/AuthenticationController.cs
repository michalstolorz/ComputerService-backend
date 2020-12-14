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

        public AuthenticationController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager,
            IMapper mapper, IConfiguration configuration, IDateTimeProvider dateTimeProvider)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _configuration = configuration;
            _dateTimeProvider = dateTimeProvider;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("login")]
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

                    var token = TokenHelper.GenerateToken(user.Id, key, _dateTimeProvider);
                    var loggedUser = _mapper.Map<LoginResponse>(user);
                    loggedUser.Token = token;

                    return Ok(loggedUser);
                }
            }

            return Unauthorized();
        }

    }
}
