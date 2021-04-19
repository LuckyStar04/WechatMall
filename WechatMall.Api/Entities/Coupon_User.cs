using System;
using System.ComponentModel.DataAnnotations;

namespace WechatMall.Api.Entities
{
    public class Coupon_User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public Guid UserID { get; set; }
        public User User { get; set; }
        [Required]
        public int CouponID { get; set; }
        public Coupon Coupon { get; set; }
    }
}
