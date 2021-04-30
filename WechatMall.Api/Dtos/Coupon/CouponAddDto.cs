using System;
using System.ComponentModel.DataAnnotations;
using WechatMall.Api.Entities;

namespace WechatMall.Api.Dtos
{
    public class CouponAddDto
    {
        [Required(ErrorMessage = "必须输入优惠券名称！")]
        public string CouponName { get; set; }
        public string ProductIDs { get; set; }   //适用范围
        [Required(ErrorMessage = "必须指定优惠券类型！")]
        public CouponType? CouponType { get; set; }
        [Required(ErrorMessage = "必须指定满减条件！")]
        public decimal? Condition { get; set; }   //满减条件（满多少）
        [Required(ErrorMessage = "必须指定满减金额！")]
        public decimal? Amount { get; set; }      //满减金额（减多少）
        [Required(ErrorMessage = "必须指定开始时间！")]
        public DateTime? StartTime { get; set; }  //开始时间
        [Required(ErrorMessage = "必须指定结束时间！")]
        public DateTime? EndTime { get; set; }    //结束时间
        [Required(ErrorMessage = "必须指定限制领取张数！")]
        public int? AllowLimit { get; set; }        //每人限制领取张数
        [Required(ErrorMessage = "必须指定总张数！")]
        public int? CouponCount { get; set; }            //总张数
    }
}
