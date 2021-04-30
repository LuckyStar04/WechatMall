using System;
using System.ComponentModel.DataAnnotations;

namespace WechatMall.Api.Entities
{
    public class Coupon_User
    {
        [Required]
        public Guid UserID { get; set; }
        public User User { get; set; }
        [Required]
        public int CouponID { get; set; }
        public Coupon Coupon { get; set; }
        public int RecievedCount { get; set; }  //已领张数
        public int RemainedCount { get; set; }  //剩余张数
    }
}
