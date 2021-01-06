using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ComputerService.Core.Models
{
    public class CreateInvoicePDFModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        [Display(Name = "Net Price")]
        public decimal NetPrice { get; set; }
        [Display(Name = "Net Value")]
        public decimal NetValue { get; set; }
        public string Tax { get; set; }
        public decimal TaxValue { get; set; }
        [Display(Name = "Gross Value")]
        public decimal SummaryPrice { get; set; }
    }
}
