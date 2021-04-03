using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WechatMall.Api.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(10)]
        public string CategoryID { get; set; }
        [Required, StringLength(10)]
        public string Name { get; set; }
        [Required, DefaultValue(0)]
        public int OrderbyId { get; set; }
        [Required, StringLength(255)]
        public string Icon { get; set; }
        [Required, DefaultValue(true)]
        public bool IsShown { get; set; } = true;
        [Required, DefaultValue(false)]
        public bool IsDeleted { get; set; } = false;
        public List<Product> Products { get; set; }
    }
}
