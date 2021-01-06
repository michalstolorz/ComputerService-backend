using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ComputerService.Core.Interfaces.Services
{
    public interface IInvoiceService
    {
        Task<int> AddInvoiceAsync(string filePath, CancellationToken cancellationToken);
    }
}
