using AutoMapper;
using ComputerService.Core.Dto.Request;
using ComputerService.Core.Interfaces.Services;
using ComputerService.Core.Models;
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

namespace ComputerService.Core.Services
{
    public class RepairService : IRepairService
    {
        private readonly IRepairRepository _repairRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RepairService(IRepairRepository repairRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor) 
        {
            _repairRepository = repairRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<int> CreateRepairAsync(CancellationToken cancelationToken)
        {
            var repair = new Repair
            {
                CreateDateTime = DateTime.Now,
                Status = EnumStatus.New,
                //UserId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value)
                UserId = 7, //////////////////////////////////////////////////////// DO ZMIANY KIEDY ZROBI SIE AUTORYZACJE
                CustomerId = 2
            };

            var result = await _repairRepository.AddAsync(repair, cancelationToken);

            return result.Id;
        }

        public async Task<RepairDetailsResponse> GetRepairAsync(int id, CancellationToken cancellationToken)
        {
            var validator = new IdValidator();
            await validator.ValidateAndThrowAsync(id, null, cancellationToken);

            var result = await _repairRepository.GetByIdAsync(id, cancellationToken,
                include: x => x
                .Include(x => x.UsedParts)
                    .ThenInclude(x => x.Part)
                .Include(x => x.RequiredRepairTypes)
                    .ThenInclude(x => x.RepairType)
                .Include(x => x.User)
                .Include(x => x.Customer)
                );

            if (result == null)
            {
                throw new ServiceException(ErrorCodes.RecruitmentCandidatetWithGivenIdNotFound, $"Repair with provided id doesn't exist");
            }

            var mapped = _mapper.Map<RepairDetailsResponse>(result);

            return mapped;
        }
    }
}
