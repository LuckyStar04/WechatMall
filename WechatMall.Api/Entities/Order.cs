using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WechatMall.Api.Entities
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(10)]
        public string OrderID { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        [Required, StringLength(10)]
        public string Status { get; set; }
        [Required]
        public DateTime OrderTime { get; set; }         //下单时间
        public DateTime? DeliverTime { get; set; }      //发货时间
        [Required, StringLength(255)]
        public string Address { get; set; }
        [Required, StringLength(50)]
        public string ReceiverName { get; set; }
        [Required, StringLength(50)]
        public string PhoneNumber { get; set; }
        [Required, StringLength(6)]
        public string PostCode { get; set; }
        [StringLength(20)]
        public string TrackingNumber { get; set; }
        [Column(TypeName = "DECIMAL(18,4)")]
        public decimal CouponAmount { get; set; }
        [Required]
        [Column(TypeName = "DECIMAL(18,4)")]
        public decimal OriginalPrice { get; set; }
        [Required]
        [Column(TypeName = "DECIMAL(18,4)")]
        public decimal PayAmount { get; set; }
        [Required]
        [Column(TypeName = "DECIMAL(18,4)")]
        public decimal ShippingFare { get; set; }
        public DateTime? PayTime { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
    }
}
