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
        /// Get user by given id
        /// </summary>
        /// <param name="userId">Id of searched user</param>
        /// <param name="cancellationToken">Propagates notification that operation should be canceled</param>
        /// <returns>User model with matching id</returns>
        [HttpGet("getUser/{userId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(UserModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUserById(int userId, CancellationToken cancellationToken)
        {
            var user = await _userService.GetUserAsync(userId, cancellationToken);

            return Ok(user);
        }

        /// <summary>
        /// Gets all user by given role
        /// </summary>
        /// <param name="role">Role of searched users</param>
        /// <param name="cancellationToken">Propagates notification that operation should be canceled</param>
        /// <returns>List of GetCustomersResponse which contain user id, first and last name, email, phone number</returns>
        [HttpGet("getUsersFromRole")]
        [ProducesResponseType(typeof(IEnumerable<GetCustomersResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUsersFromRole([FromQuery] string role, CancellationToken cancellationToken)
        {
            var result = await _userService.GetUsersFromRoleAsync(role, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Check if user is in given role
        /// </summary>
        /// <param name="role">Role for check</param>
        /// <param name="cancellationToken">Propagates notification that operation should be canceled</param>
        /// <returns>True or false depending on the check</returns>
        [HttpGet("checkUserInRole")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CheckUserInRole([FromQuery] string role, CancellationToken cancellationToken)
        {
            var result = await _userService.CheckUserInRoleAsync(role, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Chceck user role
        /// </summary>
        /// <param name="cancellationToken">Propagates notification that operation should be canceled</param>
        /// <returns>Role name assigned to current logged user</returns>
        [HttpGet("checkUserRole")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CheckUserRole(CancellationToken cancellationToken)
        {
            var result = await _userService.CheckUserRoleAsync(cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Gets collection of users from roles: customer, employee, boss
        /// </summary>
        /// <param name="cancellationToken">Propagates notification that operation should be canceled</param>
        /// <returns>Collection of responses which contains user id, name, email, phone number, role name</returns>
        [HttpGet("getUsersWithRoles")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(IEnumerable<GetUsersWithRolesResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUsersWithRoles(CancellationToken cancellationToken)
        {
            var result = await _userService.GetUsersWithRolesAsync(cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Get current logged user
        /// </summary>
        /// <param name="cancellationToken">Propagates notification that operation should be canceled</param>
        /// <returns>User model</returns>
        [HttpGet("getCurrentLoggedUser")]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCurrentLoggedUser(CancellationToken cancellationToken)
        {
            var result = await _userService.GetCurrentLoggedUserAsync(cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Update user info by new params given in request
        /// </summary>
        /// <param name="request">Requesst with user id, first and last name, email, phone number</param>
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
