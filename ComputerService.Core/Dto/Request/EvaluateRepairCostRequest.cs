using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerService.Core.Dto.Request
{
    public class EvaluateRepairCostRequest
    {
        public int RepairId { get; set; }
        public decimal RepairCost { get; set; }
    }
}
