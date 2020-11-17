using ComputerService.Data;
using ComputerService.Data.Models;
using ComputerService.Core.Interfaces.Repositories;

namespace ComputerService.Core.Repositories
{
    public class UsedPartRepository : GenericRepository<UsedPart>, IUsedPartRepository
    {
        public UsedPartRepository(ApplicationDbContext context) : base(context) { }
    }
}
