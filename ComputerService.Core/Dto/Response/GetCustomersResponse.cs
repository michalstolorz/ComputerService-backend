using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerService.Core.Dto.Response
{
    public class GetCustomersResponse
    {
        public int Id { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhoneNumber { get; set; }
    }
}
