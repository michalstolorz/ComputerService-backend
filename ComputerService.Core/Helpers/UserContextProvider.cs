using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ComputerService.Core.Helpers
{
    public class UserContextProvider : IUserContextProvider
    {
        public ClaimsPrincipal User { get; }
        public int? UserId => GetUserIdFromJwt();
        public string UserName => GetUserNameFromJwt();
        public bool Authenticated => UserId != null && UserName != null;

        public UserContextProvider(IHttpContextAccessor httpContextAccessor)
        {
            User = httpContextAccessor.HttpContext.User;
        }

        private int? GetUserIdFromJwt()
        {
            var claim = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(claim) && int.TryParse(claim, out int userId))
                return userId;

            return null;
        }

        private string GetUserNameFromJwt()
        {
            var claim = User?.FindFirst(ClaimTypes.Name)?.Value;
            if (!string.IsNullOrEmpty(claim))
                return claim;

            return null;
        }
    }
}
