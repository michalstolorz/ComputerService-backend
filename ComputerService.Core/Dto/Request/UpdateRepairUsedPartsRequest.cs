using System.Collections.Generic;

namespace ComputerService.Core.Dto.Request
{
    public class UpdateRepairUsedPartsRequest
    {
        public int RepairId { get; set; }
        public int PartId { get; set; }
        public int UsedPartQuantity { get; set; }
    }
}
