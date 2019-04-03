using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace GMSTEK.Models
{
    public class Item
    {
        public int ItemId { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        public string ImageUrl { get; set; }

        [JsonIgnore]
        public ICollection<InvoiceItem> InvoiceItems { get; set; }
    }
}
