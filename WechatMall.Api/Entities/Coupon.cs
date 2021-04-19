using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WechatMall.Api.Entities
{
    public class Coupon
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(255)]
        public string ProductIDs { get; set; }   //适用范围
        [Required]
        public CouponType CouponType { get; set; }
        [Required]
        public decimal Condition { get; set; }   //满减条件（满多少）
        [Required]
        public decimal Amount { get; set; }      //满减金额（减多少）
        [Required]
        public DateTime StartTime { get; set; }  //开始时间
        [Required]
        public DateTime EndTime { get; set; }    //结束时间
        public List<Coupon_User> Users { get; set; }
    }

    public enum CouponType
    {
        Minus = 0,  //满减
        Percent = 1 //打折
    }
}
