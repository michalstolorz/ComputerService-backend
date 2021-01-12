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
        
        public async Task<UserModel> GetUserAsync(int userId, CancellationToken cancellationToken)
        {
            var validator = new IdValidator();
            await validator.ValidateAndThrowAsync(userId, null, cancellationToken);

            var user = await _userManager.FindByIdAsync(userId.ToString());

            return _mapper.Map<UserModel>(user);
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

            var userList = customersList.Concat(employeesList).Concat(bossList).ToList();

            return userList;
        }

        public async Task<User> GetCurrentLoggedUserAsync(CancellationToken cancellationToken)
        {
            return await _userManager.GetUserAsync(_userContextProvider.User);
        }

        public async Task UpdateUserInfoAsync(UpdateUserInfoRequest request, CancellationToken cancellationToken)
        {
            var validator = new UpdateUserInfoRequestValidator(); 
            await validator.ValidateAndThrowAsync(request, null, cancellationToken);

            var userToUpdate = await _userManager.FindByIdAsync(request.UserId.ToString());
            if(userToUpdate == null)
            {
                throw new ServiceException(ErrorCodes.UserWithGivenIdNotFound, $"User with given id {request.UserId} not found");
            }

            userToUpdate.FirstName = request.FirstName;
            userToUpdate.LastName = request.LastName;
            userToUpdate.Email = request.Email;
            userToUpdate.PhoneNumber = request.PhoneNumber;

            await _userManager.UpdateAsync(userToUpdate);
        }

        private List<GetUsersWithRolesResponse> TransformListUserToResponse(IList<User> users, string role)
        {
            List<GetUsersWithRolesResponse> userList = new List<GetUsersWithRolesResponse>();
            foreach(var user in users)
            {
                GetUsersWithRolesResponse response = new GetUsersWithRolesResponse()
                {
                    UserId = user.Id,
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
