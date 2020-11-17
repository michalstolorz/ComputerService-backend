using System.Collections.Generic;

namespace ComputerService.Core.Models
{
    public class PartModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Quantity { get; set; }
        public decimal PartBoughtPrice { get; set; }
        public virtual ICollection<UsedPartModel> UsedPartsModel { get; set; }
    }
}
