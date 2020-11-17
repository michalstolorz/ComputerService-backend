
namespace ComputerService.Core.Models
{
    public class RequiredRepairTypeModel
    {
        public int Id { get; set; }
        public int RepairId { get; set; }
        public RepairModel RepairModel { get; set; }
        public int RepairTypeId { get; set; }
        public RepairTypeModel RepairTypeModel { get; set; }
    }
}
