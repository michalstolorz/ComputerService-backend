using ComputerService.Data;
using ComputerService.Data.Models;
using ComputerService.Core.Interfaces.Repositories;

namespace ComputerService.Core.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(ApplicationDbContext context) : base(context) { }
    }
}
