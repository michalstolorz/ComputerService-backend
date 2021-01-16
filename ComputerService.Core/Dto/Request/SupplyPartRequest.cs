using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerService.Core.Dto.Request
{
    public class SupplyPartRequest
    {
        public int PartId { get; set; }
        public int Quantity { get; set; }
    }
}
