using System.Collections.Generic;

namespace ComputerService.Data.Models
{
    public class RepairType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<RequiredRepairType> RequiredRepairTypes { get; set; }
    }
}
