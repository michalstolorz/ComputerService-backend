using ComputerService.Core.Dto.Request;
using ComputerService.Core.Dto.Response;
using ComputerService.Core.Models;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ComputerService.Core.Interfaces.Services
{
    public interface ICustomerService
    {
        Task<List<GetCustomersResponse>> GetCustomersAsync(GetCustomersRequest request, CancellationToken cancellationToken);
    }
}
