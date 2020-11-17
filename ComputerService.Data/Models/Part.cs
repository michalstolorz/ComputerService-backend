using System.Collections.Generic;

namespace ComputerService.Data.Models
{
    public class Part
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Quantity { get; set; }
        public decimal PartBoughtPrice { get; set; }
        public virtual ICollection<UsedPart> UsedParts { get; set; }
    }
}
