
namespace ComputerService.Data.Models
{
    public class RequiredRepairType
    {
        public int Id { get; set; }
        public int RepairId { get; set; }
        public Repair Repair { get; set; }
        public int RepairTypeId { get; set; }
        public RepairType RepairType { get; set; }
    }
}
