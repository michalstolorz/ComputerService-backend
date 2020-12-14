using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerService.Core.Dto.Request
{
    public class AddUserToRolesRequest
    {
        public int UserId { get; set; }
        public string Role { get; set; }
    }
}
