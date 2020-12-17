using ComputerService.Core.Dto.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ComputerService.Core.Interfaces.Services
{
    public interface IUserService
    {
        Task<List<GetCustomersResponse>> GetUsersFromRoleAsync(string role, CancellationToken cancellationToken);
    }
}
