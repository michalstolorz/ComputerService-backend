using AutoMapper;
using ComputerService.Core.Dto.Request;
using ComputerService.Core.Interfaces.Services;
using ComputerService.Data.Models;
using Microsoft.AspNetCore.Http;
using ComputerService.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ComputerService.Core.Interfaces.Repositories;
using System.Security.Claims;
using ComputerService.Core.Dto.Response;
using ComputerService.Core.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ComputerService.Core.Exceptions;
using System.Linq.Expressions;
using ComputerService.Core.Models;
using ComputerService.Core.Helpers;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace ComputerService.Core.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;
        private readonly IUserContextProvider _userContextProvider;

        public UserService(UserManager<User> userManager, RoleManager<Role> roleManager, IMapper mapper, IUserContextProvider userContextProvider)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _userContextProvider = userContextProvider;
        }

        public async Task<List<GetCustomersResponse>> GetUsersFromRoleAsync(string role, CancellationToken cancellationToken)
        {
            var result = await _userManager.GetUsersInRoleAsync(role);

            if (result == null)
            {
                throw new ServiceException(ErrorCodes.CustomerWithGivenIdNotFound, $"Customer with provided id doesn't exist");
            }

            return _mapper.Map<List<GetCustomersResponse>>(result);
        }

        public async Task<bool> CheckUserInRoleAsync(string role, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(_userContextProvider.User);

            return await _userManager.IsInRoleAsync(user, role);
        }

        public async Task<string> CheckUserRoleAsync(CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(_userContextProvider.User);

            var result = await _userManager.GetRolesAsync(user);

            if (result.Count == 0)
            {
                throw new ServiceException(ErrorCodes.UserNotAssignToAnyRole, $"User {user.FirstName} {user.LastName} is not assign to any role");
            }

            return result.First();
        }

        public async Task<List<GetUsersWithRolesResponse>> GetUsersWithRolesAsync(CancellationToken cancellationToken)
        {
            var customersList = TransformListUserToResponse(await _userManager.GetUsersInRoleAsync("Customer"), "Customer");
            var employeesList = TransformListUserToResponse(await _userManager.GetUsersInRoleAsync("Employee"), "Employee");
            var bossList = TransformListUserToResponse(await _userManager.GetUsersInRoleAsync("Boss"), "Boss");

            var userList = customersList.Concat(employeesList).ToList();
            userList = userList.Concat(bossList).ToList();

            return userList;
        }

        public async Task<User> GetCurrentLoggedUserAsync(CancellationToken cancellationToken)
        {
            return await _userManager.GetUserAsync(_userContextProvider.User);
        }

        private List<GetUsersWithRolesResponse> TransformListUserToResponse(IList<User> users, string role)
        {
            List<GetUsersWithRolesResponse> userList = new List<GetUsersWithRolesResponse>();
            foreach(var user in users)
            {
                GetUsersWithRolesResponse response = new GetUsersWithRolesResponse()
                {
                    UserName = user.FirstName + " " + user.LastName,
                    UserEmail = user.Email,
                    UserPhoneNumber = user.PhoneNumber,
                    RoleName = role
                };

                userList.Add(response);
            }

            return userList;
        }
    }
}
