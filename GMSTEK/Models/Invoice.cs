using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace GMSTEK.Models
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        [Required]
        public string Client { get; set; }
        [Required]
        public DateTime InvoiceDate { get; set; }

        public ICollection<InvoiceItem> InvoiceItems { get; set; }
    }
}
