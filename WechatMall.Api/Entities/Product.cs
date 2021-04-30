using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WechatMall.Api.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(10)]
        public string ProductID { get; set; }
        [StringLength(10)]
        public string CategoryID { get; set; }
        public Category Category { get; set; }
        [Required, StringLength(100)]
        public string Name { get; set; }
        public List<ProductImage> Images { get; set; } = new List<ProductImage>();
        [Required, DefaultValue(0)]
        [ConcurrencyCheck]
        public int StockCount { get; set; }
        [Required, DefaultValue(0)]
        public int SoldCount { get; set; }
        [Required, DefaultValue(0)]
        [Column(TypeName = "DECIMAL(18,4)")]
        public decimal Price { get; set; } = 0m;
        [StringLength(10)]
        public string ShippingAddress { get; set; }
        public int ShippingFareID { get; set; }
        public ShippingFare ShippingFare { get; set; }
        public string Detail { get; set; }
        [Required, DefaultValue(0)]
        public int Recommend { get; set; }
        [Required, DefaultValue(0)]
        public int OrderbyId { get; set; }
        [Required, DefaultValue(true)]
        public bool OnSale { get; set; } = true;
        [Required, DefaultValue(false)]
        public bool IsDeleted { get; set; } = false;
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
