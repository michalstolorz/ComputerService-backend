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
using System.Linq;

namespace ComputerService.Core.Services
{
    public class UsedPartService : IUsedPartService
    {
        private readonly IUsedPartRepository _usedPartRepository;
        private readonly IRepairRepository _repairRepository;
        private readonly IPartRepository _partRepository;

        public UsedPartService(IUsedPartRepository usedPartRepository, IRepairRepository repairRepository, IPartRepository partRepository)
        {
            _usedPartRepository = usedPartRepository;
            _repairRepository = repairRepository;
            _partRepository = partRepository;
        }


        public async Task UpdateRepairUsedPartsAsync(UpdateRepairUsedPartsRequest request, CancellationToken cancellationToken)
        {
            var validator = new UpdateRepairUsedPartsRequestValidator();
            await validator.ValidateAndThrowAsync(request, null, cancellationToken);

            var repairResult = await _repairRepository.GetByIdAsync(request.RepairId, cancellationToken);
            if (repairResult == null)
            {
                throw new ServiceException(ErrorCodes.RepairWithGivenIdNotFound, $"Repair with provided id doesn't exist");
            }

            var partResult = await _partRepository.GetByIdAsync(request.PartId, cancellationToken);
            if (partResult == null)
            {
                throw new ServiceException(ErrorCodes.PartWithGivenIdNotFound, $"Part with provided id doesn't exist");
            }

            if(partResult.Quantity - request.UsedPartQuantity < 0)
            {
                throw new ServiceException(ErrorCodes.NotEnoughtPartsInWarehouse, $"Not enought parts in warehouse");
            }

            if (await _usedPartRepository.AnyAsync(x =>
             x.RepairId == request.RepairId &&
             x.PartId == request.PartId, cancellationToken))
            {
                throw new ServiceException(ErrorCodes.UsedPartAlreadyAssignToRepair, $"Part with provided id {request.PartId} already assign to given repair with id {request.RepairId}");
            }

            UsedPart usedPart = new UsedPart()
            {
                RepairId = request.RepairId,
                PartId = request.PartId,
                Quantity = request.UsedPartQuantity
            };

            await _usedPartRepository.AddAsync(usedPart, cancellationToken);

            partResult.Quantity -= request.UsedPartQuantity;

            await _partRepository.UpdateAsync(cancellationToken, partResult);
        }
    }
}
