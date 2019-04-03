using System;
using System.ComponentModel.DataAnnotations;

namespace GMSTEK.Models
{
    public class InvoiceItem
    {
        public int InvoiceId { get; set; }
        public int ItemId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public double UnitValue { get; set; }

        public Invoice Invoice { get; set; }
        public Item Item { get; set; }

    }
}
