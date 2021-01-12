using ComputerService.Data.Models;
using System.Collections.Generic;

namespace ComputerService.Core.Models
{
    public class UserModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public ICollection<RepairModel> RepairsModel { get; set; }
    }
}
