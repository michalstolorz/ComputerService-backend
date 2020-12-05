using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerService.Core.Models
{
    public class CustomerModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<RepairModel> Repairs { get; set; }
    }
}
