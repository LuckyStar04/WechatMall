using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WechatMall.Api.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public Guid UserID { get; set; }
        [Required, StringLength(64)]
        public string OpenID { get; set; }
        [StringLength(64)]
        public string UnionID { get; set; }
        [StringLength(64)]
        public string SessionKey { get; set; }
        [Required, StringLength(64)]
        public string NickName { get; set; }
        [Required]
        public Gender Gender { get; set; }
        [Required, StringLength(10)]
        public string Language { get; set; }
        [Required, StringLength(64)]
        public string City { get; set; }
        [Required, StringLength(64)]
        public string Province { get; set; }
        [Required, StringLength(64)]
        public string Country { get; set; }
        [Required, StringLength(255)]
        public string AvatarUrl { get; set; }
        public List<Order> Orders { get; set; }
        public List<ShippingAddr> ShippingAddrs { get; set; }
        public List<Coupon_User> Coupons { get; set; }
    }

    public enum Gender
    {
        女 = 0,
        男 = 1,
    }
}
