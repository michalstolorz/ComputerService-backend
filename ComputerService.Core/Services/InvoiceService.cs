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
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Drawing;
using System.IO;
using Syncfusion.Pdf.Grid;
using Microsoft.AspNetCore.Hosting;

namespace ComputerService.Core.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IRepairRepository _repairRepository;

        public InvoiceService(IInvoiceRepository invoiceRepository, IRepairRepository repairRepository)
        {
            _invoiceRepository = invoiceRepository;
            _repairRepository = repairRepository;
        }

        public async Task<int> AddInvoiceAsync(string filePath, CancellationToken cancellationToken)
        {
            var validator = new StringValidator();
            await validator.ValidateAndThrowAsync(filePath, null, cancellationToken);

            var invoiceToAdd = new Invoice()
            {
                URLPath = filePath
            };

            var result = await _invoiceRepository.AddAsync(invoiceToAdd, cancellationToken);

            return result.Id;
        }
    }
}
