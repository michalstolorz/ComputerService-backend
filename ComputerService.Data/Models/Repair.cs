using ComputerService.Common.Enums;
using System;
using System.Collections.Generic;

namespace ComputerService.Data.Models
{
    public class Repair
    {
        public int Id { get; set; }
        public decimal? RepairCost { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime? FinishDateTime { get; set; }
        public EnumStatus Status { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public int? InvoiceId { get; set; }
        public Invoice Invoice { get; set; }
        public string Description { get; set; }
        public virtual ICollection<RequiredRepairType> RequiredRepairTypes { get; set; }
        public virtual ICollection<UsedPart> UsedParts { get; set; }
    }
}
