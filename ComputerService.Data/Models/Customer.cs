using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace ComputerService.Data.Models
{
    public class Customer : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<Repair> Repairs { get; set; }
    }
}
