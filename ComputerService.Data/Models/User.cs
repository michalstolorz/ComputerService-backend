using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace ComputerService.Data.Models
{
    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<Repair> Repairs { get; set; }
        public ICollection<EmployeeRepair> EmployeeRepairs { get; set; }
    }
}