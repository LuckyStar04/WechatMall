using System.ComponentModel.DataAnnotations;

namespace WechatMall.Api.Dtos
{
    public class CategoryAddDto
    {
        [Display(Name = "分类ID")]
        [Required(ErrorMessage = "{0} 字段是必填的")]
        [MaxLength(10, ErrorMessage = "{0} 的最大长度为 {1}。")]
        public string Id { get; set; }
        [Display(Name = "分类名称")]
        [Required(ErrorMessage = "{0} 字段是必填的")]
        [MaxLength(10, ErrorMessage = "{0} 的最大长度为 {1}。")]
        public string Name { get; set; }
        [Display(Name = "分类图标")]
        [Required(ErrorMessage = "{0} 字段是必填的")]
        [MaxLength(255, ErrorMessage = "{0} 的最大长度为 {1}。")]
        public string Icon { get; set; }
        [Display(Name = "排序ID")]
        public int OrderbyId { get; set; } = 0;
    }
}
