using ComputerService.Common.Enums;
using ComputerService.Data.Models;
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
        public EnumStatus Status { get; set; }
        public int CustomerId { get; set; }
        public UserModel CustomerModel { get; set; }
        public int InvoiceId { get; set; }
        public InvoiceModel InvoiceModel { get; set; }
        public string Description { get; set; }
        public virtual ICollection<RequiredRepairTypeModel> RequiredRepairTypesModel { get; set; }
        public virtual ICollection<UsedPartModel> UsedPartsModel { get; set; }
        public virtual ICollection<EmployeeRepair> EmployeeRepairsModel { get; set; }
    }
}
