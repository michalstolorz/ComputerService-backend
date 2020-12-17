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

namespace ComputerService.Core.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;

        public UserService(UserManager<User> userManager, RoleManager<Role> roleManager, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
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
    }
}
