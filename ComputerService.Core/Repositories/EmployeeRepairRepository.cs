using ComputerService.Data;
using ComputerService.Data.Models;
using ComputerService.Core.Interfaces.Repositories;

namespace ComputerService.Core.Repositories
{
    public class EmployeeRepairRepository : GenericRepository<EmployeeRepair>, IEmployeeRepairRepository
    {
        public EmployeeRepairRepository(ApplicationDbContext context) : base(context) { }
    }
}
