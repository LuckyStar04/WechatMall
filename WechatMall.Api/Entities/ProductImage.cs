using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WechatMall.Api.Entities
{
    public class ProductImage
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public Guid Guid { get; set; }
        [StringLength(10)]
        public string ProductID { get; set; }
        public Product Product { get; set; }
        [Required, StringLength(255)]
        [Column(TypeName = "VARCHAR(255)")]
        public string ImagePath { get; set; }
        [Required, StringLength(255)]
        [Column(TypeName = "VARCHAR(255)")]
        public string PhysicalPath { get; set; }
        [Required, DefaultValue(0)]
        public int OrderbyId { get; set; }
    }
}
