using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerService.Core.Dto.Request
{
    public class AssignRepairTypeToRepairRequest
    {
        public ICollection<int> RepairTypeIds{ get; set; }
    }
}
