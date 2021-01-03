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

namespace ComputerService.Core.Services
{
    public class EmployeeRepairService : IEmployeeRepairService
    {
        private readonly IEmployeeRepairRepository _employeeRepairRepository;
        private readonly IUserContextProvider _userContextProvider;

        public EmployeeRepairService(IEmployeeRepairRepository employeeRepairRepository, IUserContextProvider userContextProvider)
        {
            _employeeRepairRepository = employeeRepairRepository;
            _userContextProvider = userContextProvider;
        }

        public async Task AddEmployeeRepair(int repairId, CancellationToken cancellationToken)
        {
            var validator = new IdValidator();
            await validator.ValidateAndThrowAsync(repairId, null, cancellationToken);

            var result = await _employeeRepairRepository.GetAsync(predicate: x => x.RepairId == repairId && x.UserId == (int)_userContextProvider.UserId, cancellationToken);
            if(result != null)
            {
                throw new ServiceException(ErrorCodes.EmployeeAlreadyAssignToRepair, $"Employee with given id {(int)_userContextProvider.UserId}, already assign to repair with given id {repairId}");
            }

            var employeeRepair = new EmployeeRepair
            {
                UserId = (int)_userContextProvider.UserId,
                RepairId = repairId
            };

            await _employeeRepairRepository.AddAsync(employeeRepair, cancellationToken);
        }
    }
}
