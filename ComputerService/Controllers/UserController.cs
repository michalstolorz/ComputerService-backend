using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ComputerService.Core.Dto.Request;
using ComputerService.Core.Interfaces.Services;
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
        /// <param name="role"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("getUsersFromRole")]
        public async Task<IActionResult> GetUsersFromRole([FromQuery] string role, CancellationToken cancellationToken)
        {
            var result = await _userService.GetUsersFromRoleAsync(role, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="role"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("checkUserInRole")]
        public async Task<IActionResult> CheckUserInRole([FromQuery] string role, CancellationToken cancellationToken)
        {
            var result = await _userService.CheckUserInRoleAsync(role, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("checkUserRole")]
        public async Task<IActionResult> CheckUserRole(CancellationToken cancellationToken)
        {
            var result = await _userService.CheckUserRoleAsync(cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("getUsersWithRoles")]
        public async Task<IActionResult> GetUsersWithRoles(CancellationToken cancellationToken)
        {
            var result = await _userService.GetUsersWithRolesAsync(cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("getCurrentLoggedUser")]
        public async Task<IActionResult> GetCurrentLoggedUser(CancellationToken cancellationToken)
        {
            var result = await _userService.GetCurrentLoggedUserAsync(cancellationToken);

            return Ok(result);
        }
    }
}
