
namespace ComputerService.Core.Models
{
    public class UsedPartModel
    {
        public int Id { get; set; }
        public int RepairId { get; set; }
        public RepairModel RepairModel { get; set; }
        public int PartId { get; set; }
        public PartModel PartModel { get; set; }
    }
}
