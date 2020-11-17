using ComputerService.Data;
using ComputerService.Data.Models;
using ComputerService.Core.Interfaces.Repositories;

namespace ComputerService.Core.Repositories
{
    public class RepairRepository : GenericRepository<Repair>, IRepairRepository
    {
        public RepairRepository(ApplicationDbContext context) : base(context) { }
    }
}
