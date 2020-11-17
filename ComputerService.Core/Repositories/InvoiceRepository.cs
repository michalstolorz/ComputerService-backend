using ComputerService.Data;
using ComputerService.Data.Models;
using ComputerService.Core.Interfaces.Repositories;

namespace ComputerService.Core.Repositories
{
    public class InvoiceRepository : GenericRepository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(ApplicationDbContext context) : base(context) { }
    }
}
