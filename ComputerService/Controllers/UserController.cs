using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ComputerService.Core.Dto.Request;
using ComputerService.Core.Dto.Response;
using ComputerService.Core.Interfaces.Services;
using ComputerService.Core.Models;
using ComputerService.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComputerService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken">Propagates notification that operation should be canceled</param>
        /// <returns></returns>
        [HttpGet("getUser/{userId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(UserModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUserById(int userId, CancellationToken cancellationToken)
        {
            var user = await _userService.GetUserAsync(userId, cancellationToken);

            return Ok(user);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="role"></param>
        /// <param name="cancellationToken">Propagates notification that operation should be canceled</param>
        /// <returns></returns>
        [HttpGet("getUsersFromRole")]
        [ProducesResponseType(typeof(IEnumerable<GetCustomersResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUsersFromRole([FromQuery] string role, CancellationToken cancellationToken)
        {
            var result = await _userService.GetUsersFromRoleAsync(role, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="role"></param>
        /// <param name="cancellationToken">Propagates notification that operation should be canceled</param>
        /// <returns></returns>
        [HttpGet("checkUserInRole")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CheckUserInRole([FromQuery] string role, CancellationToken cancellationToken)
        {
            var result = await _userService.CheckUserInRoleAsync(role, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken">Propagates notification that operation should be canceled</param>
        /// <returns></returns>
        [HttpGet("checkUserRole")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CheckUserRole(CancellationToken cancellationToken)
        {
            var result = await _userService.CheckUserRoleAsync(cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken">Propagates notification that operation should be canceled</param>
        /// <returns></returns>
        [HttpGet("getUsersWithRoles")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(IEnumerable<GetUsersWithRolesResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUsersWithRoles(CancellationToken cancellationToken)
        {
            var result = await _userService.GetUsersWithRolesAsync(cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken">Propagates notification that operation should be canceled</param>
        /// <returns></returns>
        [HttpGet("getCurrentLoggedUser")]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCurrentLoggedUser(CancellationToken cancellationToken)
        {
            var result = await _userService.GetCurrentLoggedUserAsync(cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken">Propagates notification that operation should be canceled</param>
        /// <returns></returns>
        [HttpPut("updateUserInfo")]
        [Authorize(Roles = "Admin, Employee, Customer, Boss")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateUserInfo([FromBody] UpdateUserInfoRequest request, CancellationToken cancellationToken)
        {
            await _userService.UpdateUserInfoAsync(request, cancellationToken);

            return Ok();
        }
    }
}
