using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WechatMall.Api.Entities
{
    public class ShippingAddr
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public Guid UserID { get; set; }
        public User User { get; set; }
        [Required, StringLength(20)]
        public string Province { get; set; }
        [Required]
        public int ProvinceID { get; set; }
        [Required, StringLength(20)]
        public string City { get; set; }
        [Required]
        public int CityID { get; set; }
        [Required, StringLength(20)]
        public string County { get; set; }
        [Required]
        public int CountyID { get; set; }
        [Required, StringLength(255)]
        public string Address { get; set; }
        [Required, StringLength(50)]
        public string ReceiverName { get; set; }
        [Required, StringLength(50)]
        public string PhoneNumber { get; set; }
        [Required, StringLength(6)]
        public string PostCode { get; set; }
        public int OrderById { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsDefault { get; set; }
        public List<Order> Orders { get; set; }
    }
}
