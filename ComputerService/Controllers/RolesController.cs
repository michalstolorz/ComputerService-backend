using AutoMapper;
using ComputerService.Core.Dto.Request;
using ComputerService.Core.Dto.Response;
using ComputerService.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ComputerService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;

        public RolesController(UserManager<User> userManager, RoleManager<Role> roleManager, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all roles
        /// </summary>
        /// <returns>Collection of all roles</returns>
        [HttpGet("getRoles")]
        [ProducesResponseType(typeof(IEnumerable<RoleResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();

            return Ok(_mapper.Map<IEnumerable<RoleResponse>>(roles));
        }

        /// <summary>
        /// Add new role
        /// </summary>
        /// <param name="roleName">String for add</param>
        /// <returns>Newly added role</returns>
        [HttpPost("addNewRole")]
        [ProducesResponseType(typeof(IEnumerable<IdentityError>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(RoleResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddNewRole([FromBody] string roleName)
        {
            var role = new Role { Name = roleName };
            var identityResult = await _roleManager.CreateAsync(role);
            if (!identityResult.Succeeded)
            {
                return BadRequest(identityResult.Errors);
            }

            return Ok(_mapper.Map<RoleResponse>(role));
        }

        /// <summary>
        /// Get all roles from given user
        /// </summary>
        /// <param name="userId">User id for getting its roles</param>
        /// <returns>Collection of role name and its id</returns>
        [HttpGet("getUserRoles")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<RoleResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUserRoles(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return NotFound($"User with id: {userId}, doesn't exist.");
            }
            var userRolesNames = await _userManager.GetRolesAsync(user);
            var userRoles = new List<Role>();
            foreach (var userRoleName in userRolesNames)
            {
                userRoles.Add(await _roleManager.FindByNameAsync(userRoleName));
            }

            return Ok(_mapper.Map<IEnumerable<RoleResponse>>(userRoles));
        }

        /// <summary>
        /// Add user to role
        /// </summary>
        /// <param name="request">Request with user id and role</param>
        /// <returns></returns>
        [HttpPut("addUserToRoles")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<IdentityError>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddUserToRole(AddUserToRolesRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
            {
                return NotFound($"User with id: {request.UserId}, doesn't exist.");
            }

            var identityResult = await _userManager.AddToRoleAsync(user, request.Role);
            if (!identityResult.Succeeded)
            {
                return BadRequest(identityResult.Errors);
            }

            return Ok();
        }

        /// <summary>
        /// Remove user from role
        /// </summary>
        /// <param name="request">Request with user id and role id</param>
        /// <returns></returns>
        [HttpPut("removeUserFromRole")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<IdentityError>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> RemoveUserFromRole(RemoveUserFromRoleRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
            {
                return NotFound($"User with id: {request.UserId}, doesn't exist.");
            }

            var role = await _roleManager.FindByIdAsync(request.RoleId.ToString());
            var identityResult = await _userManager.RemoveFromRoleAsync(user, role.Name);
            if (!identityResult.Succeeded)
            {
                return BadRequest(identityResult.Errors);
            }

            return Ok();
        }
    }
}
