using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WechatMall.Api.Entities
{
    public class ShippingFare
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(10)]
        public string Name { get; set; }
        [Required]
        public DateTime EditTime { get; set; }
        [Required, StringLength(255)]
        public string Rules { get; set; }
        [Required, DefaultValue(false)]
        public bool IsDeleted { get; set; } = false;
        public List<Product> Products { get; set; } = new List<Product>();
    }
}
