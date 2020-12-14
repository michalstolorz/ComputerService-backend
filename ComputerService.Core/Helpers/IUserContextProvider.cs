using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace ComputerService.Core.Helpers
{
    public interface IUserContextProvider
    {
        ClaimsPrincipal User { get; }
        int? UserId { get; }
        string UserName { get; }
        bool Authenticated { get; }
    }
}
