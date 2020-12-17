using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerService.Core.Dto.Request
{
    public class UpdateRepairStatusRequest
    {
        public int RepairId { get; set; }
        public int StatusId { get; set; }
    }
}
