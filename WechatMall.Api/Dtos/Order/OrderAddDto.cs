using System;
using System.Collections.Generic;

namespace WechatMall.Api.Dtos
{
    public class OrderAddDto
    {
        public IEnumerable<OrderItemAddDto> OrderItems { get; set; }
        public string CouponID { get; set; }
        public int ShippingAddrId { get; set; }
        public DateTime OrderTime { get; set; }
    }
}
