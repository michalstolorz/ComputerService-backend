using ComputerService.Core.Dto.Request;
using ComputerService.Core.Models;
using ComputerService.Data.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ComputerService.Core.Interfaces.Services
{
    public interface IRepairTypeService
    {
        Task<IEnumerable<RepairType>> AddRepairTypeAsync(AddRepairTypeRequest request, CancellationToken cancellationToken);
        Task<RepairTypeModel> GetRepairTypeAsync(int id, CancellationToken cancellationToken);
        Task<List<RepairTypeModel>> GetRepairTypesAsync(string repairTypeName, CancellationToken cancellationToken);
    }
}
