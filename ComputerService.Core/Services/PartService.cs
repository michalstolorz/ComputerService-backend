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
    public class PartService : IPartService
    {
        private readonly IPartRepository _partRepository;
        private readonly IMapper _mapper;

        public PartService(IPartRepository partRepository, IMapper mapper)
        {
            _partRepository = partRepository;
            _mapper = mapper;
        }

        public async Task<PartModel> AddPartAsync(AddPartRequest request, CancellationToken cancellationToken)
        {
            var validator = new AddPartRequestValidator();
            await validator.ValidateAndThrowAsync(request, null, cancellationToken);

            if (await _partRepository.AnyAsync(x => x.Name == request.Name, cancellationToken))
            {
                throw new ServiceException(ErrorCodes.PartAlreadyExists, $"Part with given name already exists");
            }

            var partToAdd = new Part()
            {
                Name = request.Name,
                Quantity = request.Quantity,
                PartBoughtPrice = request.PartBoughtPrice
            };

            var result = await _partRepository.AddAsync(partToAdd, cancellationToken);

            return _mapper.Map<PartModel>(result);
        }

        public async Task<PartModel> GetPartAsync(int id, CancellationToken cancellationToken)
        {
            var validator = new IdValidator();
            await validator.ValidateAndThrowAsync(id, null, cancellationToken);

            var result = await _partRepository.GetByIdAsync(id, cancellationToken,
                include: x => x
                .Include(x => x.UsedParts));

            if (result == null)
            {
                throw new ServiceException(ErrorCodes.PartWithGivenIdNotFound, $"Part with provided id doesn't exist");
            }

            return _mapper.Map<PartModel>(result);
        }

        public async Task<List<PartModel>> GetPartsAsync(string partName, CancellationToken cancellationToken)
        {
            var result = await _partRepository.GetAsync(predicate: x =>
                (string.IsNullOrEmpty(partName) || x.Name.Contains(partName)), cancellationToken);

            if (result == null)
            {
                throw new ServiceException(ErrorCodes.PartWithGivenIdNotFound, $"Part with provided id doesn't exist");
            }

            return _mapper.Map<List<PartModel>>(result);
        }
    }
}
