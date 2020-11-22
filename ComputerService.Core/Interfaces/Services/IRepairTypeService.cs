using ComputerService.Core.Dto.Request;
using ComputerService.Data.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ComputerService.Core.Interfaces.Services
{
    public interface IRepairTypeService
    {
        Task<RepairType> AddRepairTypeAsync(AddRepairTypeRequest request, CancellationToken cancellationToken);
    }
}
