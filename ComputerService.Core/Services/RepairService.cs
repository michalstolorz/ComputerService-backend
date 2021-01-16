using AutoMapper;
using ComputerService.Core.Dto.Request;
using ComputerService.Core.Interfaces.Services;
using ComputerService.Data.Models;
using Microsoft.AspNetCore.Http;
using ComputerService.Common.Enums;
using System;
using System.Linq;
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
    public class RepairService : IRepairService
    {
        private readonly IRepairRepository _repairRepository;
        private readonly IMapper _mapper;
        private readonly IUserContextProvider _userContextProvider;
        private readonly IEmployeeRepairRepository _employeeRepairRepository;

        public RepairService(IRepairRepository repairRepository, IMapper mapper, IUserContextProvider userContextProvider, IEmployeeRepairRepository employeeRepairRepository)
        {
            _repairRepository = repairRepository;
            _mapper = mapper;
            _userContextProvider = userContextProvider;
            _employeeRepairRepository = employeeRepairRepository;
        }

        public async Task<int> AddRepairAsync(AddRepairRequest request, CancellationToken cancellationToken)
        {
            var validator = new IdValidator();
            await validator.ValidateAndThrowAsync(request.CustomerId);

            var repair = new Repair
            {
                CreateDateTime = DateTime.Now,
                Status = EnumStatus.New,
                CustomerId = request.CustomerId,
                Description = request.Description
            };

            var result = await _repairRepository.AddAsync(repair, cancellationToken);

            return result.Id;
        }

        public async Task<GetRepairDetailsResponse> GetRepairAsync(int id, CancellationToken cancellationToken)
        {
            var validator = new IdValidator();
            await validator.ValidateAndThrowAsync(id, null, cancellationToken);

            var result = await _repairRepository.GetByIdAsync(id, cancellationToken,
                include: x => x
                .Include(x => x.UsedParts)
                    .ThenInclude(x => x.Part)
                .Include(x => x.RequiredRepairTypes)
                    .ThenInclude(x => x.RepairType)
                .Include(x => x.Customer)
                .Include(x => x.EmployeeRepairs)
                    .ThenInclude(x => x.User)
                 .Include(x => x.Invoice));

            if (result == null)
            {
                throw new ServiceException(ErrorCodes.RepairWithGivenIdNotFound, $"Repair with provided id doesn't exist");
            }

            return _mapper.Map<GetRepairDetailsResponse>(result);
        }

        public async Task<GetRepairDetailsResponse> GetCustomerRepairAsync(int id, CancellationToken cancellationToken)
        {
            var validator = new IdValidator();
            await validator.ValidateAndThrowAsync(id, null, cancellationToken);

            var result = await _repairRepository.GetAsync(predicate:
                x => x.Id == id &&
                x.CustomerId == _userContextProvider.UserId,
                cancellationToken,
                include: x => x
                .Include(x => x.UsedParts)
                    .ThenInclude(x => x.Part)
                .Include(x => x.RequiredRepairTypes)
                    .ThenInclude(x => x.RepairType)
                .Include(x => x.Customer)
                .Include(x => x.EmployeeRepairs)
                    .ThenInclude(x => x.User)
                 .Include(x => x.Invoice));

            if (result == null)
            {
                throw new ServiceException(ErrorCodes.RepairWithGivenIdNotFound, $"Repair with provided id doesn't exist");
            }

            return _mapper.Map<GetRepairDetailsResponse>(result.FirstOrDefault());
        }

        public async Task<List<GetRepairsResponse>> GetRepairsByStatusAsync(int repairStatusId, CancellationToken cancellationToken)
        {
            var validator = new IdValidator();
            await validator.ValidateAndThrowAsync(repairStatusId, null, cancellationToken);

            var result = await _repairRepository.GetAsync(predicate: x => x.Status == (EnumStatus)repairStatusId, cancellationToken,
                include: x => x
                .Include(x => x.Customer)
                .Include(x => x.EmployeeRepairs)
                    .ThenInclude(x => x.User));

            if (result == null)
            {
                throw new ServiceException(ErrorCodes.RepairWithGivenIdNotFound, $"Repair with provided id doesn't exist");
            }

            return _mapper.Map<List<GetRepairsResponse>>(result);
        }

        public async Task<List<GetRepairsResponse>> GetRepairsAsync(GetRepairsRequest request, CancellationToken cancellationToken)
        {
            var predicate = CreatePredicate(request);

            var result = await _repairRepository.GetAsync(predicate, cancellationToken,
                include: x => x
                .Include(x => x.Customer)
                .Include(x => x.EmployeeRepairs)
                    .ThenInclude(x => x.User),
                orderBy: x => x.OrderBy(x => x.Status));

            if (result == null)
            {
                throw new ServiceException(ErrorCodes.RepairWithGivenIdNotFound, $"Repair with provided id doesn't exist");
            }

            return _mapper.Map<List<GetRepairsResponse>>(result);
        }

        public async Task<List<GetRepairsResponse>> GetCustomerRepairsAsync(CancellationToken cancellationToken)
        {
            var result = await _repairRepository.GetAsync(predicate:
                x => x.CustomerId == _userContextProvider.UserId,
                cancellationToken,
                include: x => x
                .Include(x => x.Customer)
                .Include(x => x.EmployeeRepairs)
                    .ThenInclude(x => x.User),
                orderBy: x => x.OrderBy(x => x.Status));

            if (result == null)
            {
                throw new ServiceException(ErrorCodes.RepairWithGivenIdNotFound, $"Repair with provided id doesn't exist");
            }

            return _mapper.Map<List<GetRepairsResponse>>(result);
        }

        public async Task<List<GetRepairsResponse>> GetRepairsForInvoicesAsync(CancellationToken cancellationToken)
        {
            var result = await _repairRepository.GetAsync(predicate: x => x.Status != (EnumStatus)4 && x.RepairCost != null, cancellationToken,
                include: x => x
                .Include(x => x.Customer)
                .Include(x => x.EmployeeRepairs)
                    .ThenInclude(x => x.User));

            if (result == null)
            {
                throw new ServiceException(ErrorCodes.RepairWithGivenIdNotFound, $"Repair with provided id doesn't exist");
            }

            return _mapper.Map<List<GetRepairsResponse>>(result);
        }

        //public async Task<List<GetRepairsResponse>> GetCustomerRepairsAsync(int customerId, CancellationToken cancellationToken)
        //{
        //    var validator = new IdValidator();
        //    await validator.ValidateAndThrowAsync(customerId, null, cancellationToken);

        //    var result = await _repairRepository.GetAsync(predicate: x => x.CustomerId == customerId, cancellationToken,
        //        include: x => x
        //        .Include(x => x.Customer)
        //        .Include(x => x.EmployeeRepairs)
        //            .ThenInclude(x => x.User));

        //    if (result == null)
        //    {
        //        throw new ServiceException(ErrorCodes.RepairWithGivenIdNotFound, $"Repair with provided id doesn't exist");
        //    }

        //    return _mapper.Map<List<GetRepairsResponse>>(result);
        //}

        public async Task UpdateRepairDescriptionAsync(UpdateRepairDescriptionRequest request, CancellationToken cancellationToken)
        {
            var validator = new UpdateRepairDescriptionRequestValidator();
            await validator.ValidateAndThrowAsync(request, null, cancellationToken);

            var result = await _repairRepository.GetByIdAsync(request.RepairId, cancellationToken);
            if (result == null)
            {
                throw new ServiceException(ErrorCodes.RepairWithGivenIdNotFound, $"Repair with provided id doesn't exist");
            }

            result.Description = request.NewDescription;

            await _repairRepository.UpdateAsync(cancellationToken, result);
        }

        public async Task EvaluateRepairCostAsync(EvaluateRepairCostRequest request, CancellationToken cancellationToken)
        {
            var validator = new EvaluateRepairCostRequestValidator();
            await validator.ValidateAndThrowAsync(request, null, cancellationToken);

            var result = await _repairRepository.GetByIdAsync(request.RepairId, cancellationToken);
            if (result == null)
            {
                throw new ServiceException(ErrorCodes.RepairWithGivenIdNotFound, $"Repair with provided id doesn't exist");
            }

            result.RepairCost = request.RepairCost;

            await _repairRepository.UpdateAsync(cancellationToken, result);
        }

        public async Task<RepairModel> UpdateRepairStatusAsync(UpdateRepairStatusRequest request, CancellationToken cancellationToken)
        {
            var validator = new IdValidator();
            await validator.ValidateAndThrowAsync(request.RepairId, null, cancellationToken);
            await validator.ValidateAndThrowAsync(request.StatusId, null, cancellationToken);

            var repairToUpdate = await _repairRepository.GetByIdAsync(request.RepairId, cancellationToken);
            if (repairToUpdate == null)
            {
                throw new ServiceException(ErrorCodes.RepairWithGivenIdNotFound, $"Repair with provided id doesn't exist");
            }

            repairToUpdate.Status = (EnumStatus)request.StatusId;

            var result = await _repairRepository.UpdateAsync(cancellationToken, repairToUpdate);

            return _mapper.Map<RepairModel>(result);
        }

        private Expression<Func<Repair, bool>> CreatePredicate(GetRepairsRequest request)
        {
            Expression<Func<Repair, bool>> predicate = x =>
                (!request.Id.HasValue || x.Id.Equals(request.Id)) &&
                (!request.CustomerId.HasValue || x.CustomerId.Equals(request.CustomerId)) &&
                (string.IsNullOrEmpty(request.CustomerFirstName) || x.Customer.FirstName.Contains(request.CustomerFirstName)) &&
                (string.IsNullOrEmpty(request.CustomerLastName) || x.Customer.LastName.Contains(request.CustomerLastName)) &&
                (string.IsNullOrEmpty(request.CustomerEmail) || x.Customer.Email.Contains(request.CustomerEmail)) &&
                (string.IsNullOrEmpty(request.CustomerPhoneNumber) || x.Customer.PhoneNumber.Contains(request.CustomerPhoneNumber));

            return predicate;
        }
    }
}
