using System;
using System.Collections.Generic;

namespace WechatMall.Api.Dtos
{
    public class OrderDetailDto
    {
        public string OrderID { get; set; }
        public Guid UserID { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
        public string Status { get; set; }
        public DateTime OrderTime { get; set; }         //下单时间
        public DateTime? DeliverTime { get; set; }      //发货时间
        public string TrackingNumber { get; set; }
        public decimal CouponAmount { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal PayAmount { get; set; }
        public decimal ShippingFare { get; set; }
        public DateTime? PayTime { get; set; }
    }
}
