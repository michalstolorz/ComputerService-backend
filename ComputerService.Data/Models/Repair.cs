using System;
using System.Collections.Generic;

namespace ComputerService.Data.Models
{
    public class Repair
    {
        public int Id { get; set; }
        public decimal RepairCost { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime FinishDateTime { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; }
        public virtual ICollection<RequiredRepairType> RequiredRepairTypes { get; set; }
        public virtual ICollection<UsedPart> UsedParts { get; set; }
    }
}
