using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WechatMall.Api.Entities
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(20)]
        public string OrderID { get; set; }
        [Required]
        public Guid UserID { get; set; }
        public User User { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        [Required, StringLength(10)]
        public string Status { get; set; }
        [Required]
        public DateTime OrderTime { get; set; }         //下单时间
        public DateTime? DeliverTime { get; set; }      //发货时间
        [Required]
        public int ShippingAddrId { get; set; }
        public ShippingAddr ShippingAddr { get; set; }
        [StringLength(20)]
        public string TrackingNumber { get; set; }
        [Column(TypeName = "DECIMAL(18,4)")]
        public decimal CouponAmount { get; set; }
        [Required]
        [Column(TypeName = "DECIMAL(18,4)")]
        public decimal OriginalPrice { get; set; }
        [Column(TypeName = "DECIMAL(18,4)")]
        public decimal? PayAmount { get; set; }
        [Required]
        [Column(TypeName = "DECIMAL(18,4)")]
        public decimal ShippingFare { get; set; }
        public DateTime? PayTime { get; set; }
        [Required, DefaultValue(false)]
        public bool IsDeleted { get; set; }
    }
}
