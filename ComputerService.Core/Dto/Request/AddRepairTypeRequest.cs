using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerService.Core.Dto.Request
{
    public class AddRepairTypeRequest
    {
        public ICollection<string> RepairTypeNames { get; set; }
    }
}
