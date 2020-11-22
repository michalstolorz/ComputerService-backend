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

namespace ComputerService.Core.Services
{
    public class RepairTypeService : IRepairTypeService
    {
        private readonly IRepairTypeRepository _repairTypeRepository;

        public RepairTypeService(IRepairTypeRepository repairTypeRepository)
        {
            _repairTypeRepository = repairTypeRepository;
        }

        public async Task<RepairType> AddRepairTypeAsync(AddRepairTypeRequest request, CancellationToken cancellationToken)
        {
            var validator = new StringValidator();
            await validator.ValidateAndThrowAsync(request.RepairTypeName, null, cancellationToken);

            var repairType = new RepairType()
            {
                Name = request.RepairTypeName
            };

            var result = await _repairTypeRepository.AddAsync(repairType, cancellationToken);

            return result;
        }
    }
}
