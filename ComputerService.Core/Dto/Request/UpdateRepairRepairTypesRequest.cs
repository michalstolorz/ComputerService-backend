using System.Collections.Generic;

namespace ComputerService.Core.Dto.Request
{
    public class UpdateRepairRepairTypesRequest
    {
        public int RepairId { get; set; }
        public ICollection<int> RepairTypeIds { get; set; }
    }
}
