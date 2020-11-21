using ComputerService.Common.Enums;
using System;
using System.Collections.Generic;

namespace ComputerService.Core.Models
{
    public class RepairModel
    {
        public int Id { get; set; }
        public decimal RepairCost { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime FinishDateTime { get; set; }
        public int UserId { get; set; }
        public UserModel UserModel { get; set; }
        public int InvoiceId { get; set; }
        public InvoiceModel InvoiceModel { get; set; }
        public EnumStatus Status;
        public virtual ICollection<RequiredRepairTypeModel> RequiredRepairTypesModel { get; set; }
        public virtual ICollection<UsedPartModel> UsedPartsModel { get; set; }
    }
}
