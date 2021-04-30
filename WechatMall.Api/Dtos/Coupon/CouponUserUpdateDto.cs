using System;

namespace WechatMall.Api.Dtos
{
    public class CouponUserUpdateDto
    {
        public int RecievedCount { get; set; }  //已领张数
        public int RemainedCount { get; set; }  //剩余张数
    }
}
