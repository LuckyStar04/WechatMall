using System.ComponentModel.DataAnnotations;

namespace WechatMall.Api.Dtos
{
    public class CategoryUpdateDto
    {
        [Display(Name = "分类ID")]
        [Required(ErrorMessage = "{0} 是必填项")]
        public string Id { get; set; }
        [Display(Name = "分类名称")]
        [Required(ErrorMessage = "{0} 是必填项")]
        public string Name { get; set; }
        [Display(Name = "分类图标")]
        [Required(ErrorMessage = "{0} 是必填项")]
        public string Icon { get; set; }
        [Display(Name = "分类排序ID")]
        [Required(ErrorMessage = "{0} 是必填项")]
        public int OrderbyId { get; set; }
        [Display(Name = "是否显示")]
        public bool IsShown { get; set; } = true;
        [Display(Name = "是否删除")]
        public bool IsDeleted { get; set; } = false;
    }
}
