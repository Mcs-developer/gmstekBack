using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GMSTEK.DTO
{
    public class InvoiceDTO
    {
        [Required]
        public string ClientName { get; set; }

        public ICollection<ItemDTO> Items { get; set; }

    }
}
