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

namespace ComputerService.Core.Services
{
    public class RequiredRepairTypeService : IRequiredRepairTypeService
    {
        private readonly IRequiredRepairTypeRepository _requiredRepairTypeRepository;
        private readonly IRepairRepository _repairRepository;

        public RequiredRepairTypeService(IRequiredRepairTypeRepository requiredRepairTypeRepository, IRepairRepository repairRepository)
        {
            _requiredRepairTypeRepository = requiredRepairTypeRepository;
            _repairRepository = repairRepository;
        }

        public async Task AssignRepairTypeToRepairAsync(AssignRepairTypeToRepairRequest request, CancellationToken cancellationToken)
        {
            var validator = new IdValidator();
            var listValidator = new ListIdsValidator();
            await validator.ValidateAndThrowAsync(request.RepairId, null, cancellationToken);
            await listValidator.ValidateAndThrowAsync(request.RepairTypeIds, null, cancellationToken);

            var repairResult = await _repairRepository.GetByIdAsync(request.RepairId, cancellationToken);
            if (repairResult == null)
            {
                throw new ServiceException(ErrorCodes.RepairWithGivenIdNotFound, $"Repair with provided id doesn't exist");
            }

            foreach (var repairTypeId in request.RepairTypeIds)
            {
                var repairTypeResult = await _requiredRepairTypeRepository.GetByIdAsync(repairTypeId, cancellationToken);
                if (repairTypeResult == null)
                {
                    throw new ServiceException(ErrorCodes.RepairTypeAlreadyAssignToRepair, $"Repair type with provided id {repairTypeId} doesn't exist");
                }

                if (await _requiredRepairTypeRepository.AnyAsync(x =>
                 x.RepairId == request.RepairId &&
                 x.RepairTypeId == repairTypeId, cancellationToken))
                {
                    throw new ServiceException(ErrorCodes.RepairTypeAlreadyAssignToRepair, $"Repair type with provided id {repairTypeId} already assign to given repair with id {request.RepairId}");
                }
            }

            List<RequiredRepairType> repairTypesToAssign = new List<RequiredRepairType>();
            foreach (var repairTypeId in request.RepairTypeIds)
            {
                RequiredRepairType repairType = new RequiredRepairType()
                {
                    RepairId = request.RepairId,
                    RepairTypeId = repairTypeId
                };
                repairTypesToAssign.Add(repairType);
            }

            await _requiredRepairTypeRepository.AddRangeAsync(repairTypesToAssign, cancellationToken);
        }
    }
}
