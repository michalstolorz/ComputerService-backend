
namespace ComputerService.Data.Models
{
    public class UsedPart
    {
        public int Id { get; set; }
        public int RepairId { get; set; }
        public Repair Repair { get; set; }
        public int PartId { get; set; }
        public Part Part { get; set; }
    }
}
