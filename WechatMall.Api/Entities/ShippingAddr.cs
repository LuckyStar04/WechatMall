using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WechatMall.Api.Entities
{
    public class ShippingAddr
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public Guid UserID { get; set; }
        public User User { get; set; }
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
        public List<Order> Orders { get; set; }
    }
}
