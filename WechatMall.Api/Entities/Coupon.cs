using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WechatMall.Api.Entities
{
    public class Coupon
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(40)]
        public string CouponName { get; set; }
        [Required, StringLength(255)]
        public string ProductIDs { get; set; }   //适用范围
        [Required]
        public CouponType CouponType { get; set; }
        [Required]
        [Column(TypeName = "DECIMAL(18,4)")]
        public decimal Condition { get; set; }   //满减条件（满多少）
        [Required]
        [Column(TypeName = "DECIMAL(18,4)")]
        public decimal Amount { get; set; }      //满减金额（减多少）
        [Required]
        public DateTime StartTime { get; set; }  //开始时间
        [Required]
        public DateTime EndTime { get; set; }    //结束时间
        [Required]
        public int AllowLimit { get; set; }        //每人限制领取张数
        [Required]
        [ConcurrencyCheck]
        public int CouponCount { get; set; }           //总张数
        [Required]
        public bool IsDeleted { get; set; }
        public List<Coupon_User> Users { get; set; }
    }

    public enum CouponType
    {
        Minus = 0,  //满减
        Percent = 1 //打折
    }
}
