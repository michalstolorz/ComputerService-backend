using ComputerService.Core.Dto.Request;
using ComputerService.Core.Dto.Response;
using ComputerService.Core.Models;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ComputerService.Core.Interfaces.Services
{
    public interface IPartService
    {
        Task<PartModel> AddPartAsync(AddPartRequest request, CancellationToken cancellationToken);
        Task<PartModel> GetPartAsync(int id, CancellationToken cancellationToken);
        Task<List<PartModel>> GetPartsAsync(string partName, CancellationToken cancellationToken);
        Task SupplyPartAsync(SupplyPartRequest request, CancellationToken cancellationToken);
    }
}
