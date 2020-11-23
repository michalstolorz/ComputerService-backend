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
    public class RepairTypeService : IRepairTypeService
    {
        private readonly IRepairTypeRepository _repairTypeRepository;
        private readonly IMapper _mapper;

        public RepairTypeService(IRepairTypeRepository repairTypeRepository, IMapper mapper)
        {
            _repairTypeRepository = repairTypeRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RepairType>> AddRepairTypeAsync(AddRepairTypeRequest request, CancellationToken cancellationToken)
        {
            var validator = new ListStringsValidator();
            await validator.ValidateAndThrowAsync(request.RepairTypeNames, null, cancellationToken);

            foreach(var repairTypeName in request.RepairTypeNames)
            {
                if(await _repairTypeRepository.AnyAsync(x => x.Name == repairTypeName, cancellationToken))
                {
                    throw new ServiceException(ErrorCodes.RepairTypeAlreadyExists, $"Repair type with given name already exist");
                }
            }

            var repairTypesToAdd = new List<RepairType>();
            foreach (var repairTypeName in request.RepairTypeNames)
            {
                var repairType = new RepairType(){ Name = repairTypeName };
                repairTypesToAdd.Add(repairType);
            }

            var result = await _repairTypeRepository.AddRangeAsync(repairTypesToAdd, cancellationToken);

            return result;
        }

        public async Task<RepairTypeModel> GetRepairTypeAsync(int id, CancellationToken cancellationToken)
        {
            var validator = new IdValidator();
            await validator.ValidateAndThrowAsync(id, null, cancellationToken);

            var result = await _repairTypeRepository.GetByIdAsync(id, cancellationToken,
                include: x => x
                .Include(x => x.RequiredRepairTypes));

            if (result == null)
            {
                throw new ServiceException(ErrorCodes.RepairTypeWithGivenIdNotFound, $"Repair type with provided id doesn't exist");
            }

            return _mapper.Map<RepairTypeModel>(result);
        }

        public async Task<List<RepairTypeModel>> GetRepairTypesAsync(string repairTypeName, CancellationToken cancellationToken)
        {
            var result = await _repairTypeRepository.GetAsync(predicate: x =>
                (string.IsNullOrEmpty(repairTypeName) || x.Name.Contains(repairTypeName)), cancellationToken);

            if (result == null)
            {
                throw new ServiceException(ErrorCodes.RepairTypeWithGivenIdNotFound, $"Repair type with provided id doesn't exist");
            }

            return _mapper.Map<List<RepairTypeModel>>(result);
        }
    }
}
