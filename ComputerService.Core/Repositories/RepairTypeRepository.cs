using ComputerService.Data;
using ComputerService.Data.Models;
using ComputerService.Core.Interfaces.Repositories;

namespace ComputerService.Core.Repositories
{
    public class RepairTypeRepository : GenericRepository<RepairType>, IRepairTypeRepository
    {
        public RepairTypeRepository(ApplicationDbContext context) : base(context) { }
    }
}
