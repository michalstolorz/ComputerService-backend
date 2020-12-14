using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerService.Core.Dto.Request
{
    public class RemoveUserFromRoleRequest
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
}
