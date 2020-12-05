using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerService.Core.Dto.Request
{
    public class UpdateRepairDescriptionRequest
    {
        public int RepairId { get; set; }
        public string NewDescription { get; set; }
    }
}
