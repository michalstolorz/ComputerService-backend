
namespace ComputerService.Core.Dto.Response
{
    public class LoginResponse
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public bool IsActive { get; set; }
        public string Token { get; set; }
    }
}
