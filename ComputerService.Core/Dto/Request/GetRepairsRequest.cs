
namespace ComputerService.Core.Dto.Request
{
    public class GetRepairsRequest
    {
        public int? Id { get; set; }
        public string Description { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhoneNumber { get; set; }
    }
}
