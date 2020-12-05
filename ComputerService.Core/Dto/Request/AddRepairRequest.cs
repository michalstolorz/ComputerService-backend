using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerService.Core.Dto.Request
{
    public class AddRepairRequest
    {
        public int CustomerId { get; set; }
        public string Description { get; set; }
        public ICollection<int> RepairTypeIds { get; set; }
    }
}
