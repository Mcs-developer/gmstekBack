using System;
using System.ComponentModel.DataAnnotations;

namespace GMSTEK.DTO
{
    public class ItemDTO
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public double UnitValue { get; set; }
    }
}
