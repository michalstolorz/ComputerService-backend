using ComputerService.Data;
using ComputerService.Data.Models;
using ComputerService.Core.Interfaces.Repositories;

namespace ComputerService.Core.Repositories
{
    public class RequiredRepairTypeRepository : GenericRepository<RequiredRepairType>, IRequiredRepairTypeRepository
    {
        public RequiredRepairTypeRepository(ApplicationDbContext context) : base(context) { }
    }
}
