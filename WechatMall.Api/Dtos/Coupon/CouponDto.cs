using System;
using WechatMall.Api.Entities;

namespace WechatMall.Api.Dtos
{
    public class CouponDto
    {
        public int Id { get; set; }
        public string CouponName { get; set; }
        public string ProductIDs { get; set; }   //适用范围
        public CouponType CouponType { get; set; }
        public decimal Condition { get; set; }   //满减条件（满多少）
        public decimal Amount { get; set; }      //满减金额（减多少）
        public DateTime StartTime { get; set; }  //开始时间
        public DateTime EndTime { get; set; }    //结束时间
        public int AllowLimit { get; set; }        //每人限制领取张数
        public int CouponCount { get; set; }           //总张数
        public bool IsDeleted { get; set; }
    }
}
