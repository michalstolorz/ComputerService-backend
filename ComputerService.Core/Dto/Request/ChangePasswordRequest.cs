
namespace ComputerService.Core.Dto.Request
{
    public class ChangePasswordRequest
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string NewPasswordConfirmation { get; set; }
    }
}
