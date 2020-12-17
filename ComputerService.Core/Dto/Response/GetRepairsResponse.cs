using System;
using System.Collections.Generic;

namespace ComputerService.Core.Dto.Response
{
    public class GetRepairsResponse
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime FinishDateTime { get; set; }
        public int CustomerId { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public ICollection<RepairUsers> RepairUsers { get; set; }
    }
}
