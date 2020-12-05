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
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomerService(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<List<GetCustomersResponse>> GetCustomersAsync(GetCustomersRequest request, CancellationToken cancellationToken)
        {
            var predicate = CreatePredicate(request);

            var result = await _customerRepository.GetAsync(predicate, cancellationToken);

            if (result == null)
            {
                throw new ServiceException(ErrorCodes.CustomerWithGivenIdNotFound, $"Customer with provided id doesn't exist");
            }

            return _mapper.Map<List<GetCustomersResponse>>(result);
        }

        private Expression<Func<Customer, bool>> CreatePredicate(GetCustomersRequest request)
        {
            Expression<Func<Customer, bool>> predicate = x =>
                (!request.Id.HasValue || x.Id.Equals(request.Id)) &&
                (string.IsNullOrEmpty(request.CustomerFirstName) || x.FirstName.Contains(request.CustomerFirstName)) &&
                (string.IsNullOrEmpty(request.CustomerLastName) || x.LastName.Contains(request.CustomerLastName)) &&
                (string.IsNullOrEmpty(request.CustomerEmail) || x.Email.Contains(request.CustomerEmail)) &&
                (string.IsNullOrEmpty(request.CustomerPhoneNumber) || x.PhoneNumber.Contains(request.CustomerPhoneNumber));

            return predicate;
        }
    }
}
