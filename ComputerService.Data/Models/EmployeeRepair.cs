
namespace ComputerService.Data.Models
{
    public class EmployeeRepair
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int RepairId { get; set; }
        public Repair Repair { get; set; }
    }
}
