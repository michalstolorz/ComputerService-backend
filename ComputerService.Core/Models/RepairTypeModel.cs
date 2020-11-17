using System.Collections.Generic;

namespace ComputerService.Core.Models
{
    public class RepairTypeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<RequiredRepairTypeModel> RequiredRepairTypesModel { get; set; }
    }
}
