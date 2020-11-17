using ComputerService.Data;
using ComputerService.Data.Models;
using ComputerService.Core.Interfaces.Repositories;

namespace ComputerService.Core.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) :base(context) { }
    }
}
