using ComputerService.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerService.Core.Dto.Response
{
    public class GetUsersWithRolesResponse
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPhoneNumber { get; set; }
        public string RoleName { get; set; }
    }
}
