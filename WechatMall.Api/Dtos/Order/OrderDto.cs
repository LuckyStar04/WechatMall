using System;
using WechatMall.Api.Entities;

namespace WechatMall.Api.Dtos
{
    public class OrderDto
    {
        public string OrderID { get; set; }
        public Guid UserID { get; set; }
        public string ProductImage { get; set; }
        public int OrderItemsCount { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime OrderTime { get; set; }         //下单时间
        public DateTime? DeliverTime { get; set; }      //发货时间
        public string TrackingNumber { get; set; }
        public decimal CouponAmount { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal PaidPrice { get; set; }
        public decimal ShippingFare { get; set; }
        public DateTime? PayTime { get; set; }
    }
}
