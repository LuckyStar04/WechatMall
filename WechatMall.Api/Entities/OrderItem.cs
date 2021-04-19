using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WechatMall.Api.Entities
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(15)]
        public string OrderID { get; set; }
        public Order Order { get; set; }
        [Required, StringLength(10)]
        public string ProductID { get; set; }
        public Product Product { get; set; }
        [Required]
        [Column(TypeName = "DECIMAL(18,4)")]
        public decimal Price { get; set; }
        [Required]
        public int Amount { get; set; }
    }
}
