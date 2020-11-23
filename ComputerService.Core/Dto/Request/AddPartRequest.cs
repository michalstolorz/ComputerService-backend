using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerService.Core.Dto.Request
{
    public class AddPartRequest
    {
        public string Name { get; set; }
        public decimal Quantity { get; set; }
        public decimal PartBoughtPrice { get; set; }
    }
}
