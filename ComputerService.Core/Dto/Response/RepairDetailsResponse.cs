using ComputerService.Common.Enums;
using ComputerService.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerService.Core.Dto.Response
{
    public class RepairDetailsResponse
    {
        public int Id { get; set; }
        public decimal RepairCost { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime? FinishDateTime { get; set; }
        public EnumStatus Status { get; set; }
        public int UserId { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public int CustomerId { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; }
        public ICollection<Part> PartsUsedInRepair { get; set; }
        public ICollection<RepairType> RepairTypes { get; set; }
    }
}
