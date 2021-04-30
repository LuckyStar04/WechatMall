using System;

namespace WechatMall.Api.Dtos
{
    public class CouponUserDto
    {
        public int CouponID { get; set; }
        public Guid UserID { get; set; }
        public CouponDto Coupon { get; set; }
        public int RecievedCount { get; set; }  //已领张数
        public int RemainedCount { get; set; }  //剩余张数
    }
}
