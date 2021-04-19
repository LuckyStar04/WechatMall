using System.ComponentModel.DataAnnotations;

namespace WechatMall.Api.Dtos
{
    public class ProductUpdateDto
    {
        [Display(Name = "产品ID")]
        [Required(ErrorMessage = "{0} 字段是必填的")]
        [MaxLength(10, ErrorMessage = "{0} 的最大长度为 {1}。")]
        public string ProductID { get; set; }
        [Display(Name = "分类ID")]
        [MaxLength(10, ErrorMessage = "{0} 的最大长度为 {1}。")]
        public string CategoryID { get; set; }
        [Display(Name = "产品名称")]
        [Required(ErrorMessage = "{0} 字段是必填的")]
        [MaxLength(100, ErrorMessage = "{0} 的最大长度为 {1}。")]
        public string Name { get; set; }
        [Display(Name = "剩余库存")]
        [Required(ErrorMessage = "{0} 字段是必填的")]
        public int StockCount { get; set; }
        [Display(Name = "产品价格")]
        [Required(ErrorMessage = "{0} 字段是必填的")]
        public decimal Price { get; set; } = 0m;
        [Display(Name = "发货地")]
        [MaxLength(10, ErrorMessage = "{0} 的最大长度为 {1}。")]
        public string ShippingAddress { get; set; }
        [Display(Name = "运费ID")]
        [Required(ErrorMessage = "{0} 字段是必填的")]
        public int ShippingFareID { get; set; }
        [Display(Name = "产品详情")]
        public string Detail { get; set; }
        [Display(Name = "排序ID")]
        [Required(ErrorMessage = "{0} 字段是必填的")]
        public int OrderbyId { get; set; }
        [Display(Name = "是否上架")]
        [Required(ErrorMessage = "{0} 字段是必填的")]
        public bool OnSale { get; set; } = true;
        [Display(Name = "是否删除")]
        [Required(ErrorMessage = "{0} 字段是必填的")]
        public bool IsDeleted { get; set; } = false;
    }
}
