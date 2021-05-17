using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WechatMall.Api.Dtos
{
    public class AddrUpdateDto
    {
        [Display(Name = "省名")]
        [Required(ErrorMessage = "{0} 字段是必填的")]
        [MaxLength(20, ErrorMessage = "{0} 的最大长度为 {1}。")]
        public string Province { get; set; }
        [Display(Name = "省名ID")]
        [Required(ErrorMessage = "{0} 字段是必填的")]
        public int ProvinceID { get; set; }
        [Display(Name = "市名")]
        [Required(ErrorMessage = "{0} 字段是必填的")]
        [MaxLength(20, ErrorMessage = "{0} 的最大长度为 {1}。")]
        public string City { get; set; }
        [Display(Name = "市名ID")]
        [Required(ErrorMessage = "{0} 字段是必填的")]
        public int CityID { get; set; }
        [Display(Name = "区名")]
        [Required(ErrorMessage = "{0} 字段是必填的")]
        [MaxLength(20, ErrorMessage = "{0} 的最大长度为 {1}。")]
        public string County { get; set; }
        [Display(Name = "区名ID")]
        [Required(ErrorMessage = "{0} 字段是必填的")]
        public int CountyID { get; set; }
        [Display(Name = "详细地址")]
        [Required(ErrorMessage = "{0} 字段是必填的")]
        [MaxLength(255, ErrorMessage = "{0} 的最大长度为 {1}。")]
        public string Address { get; set; }
        [Display(Name = "收件人")]
        [Required(ErrorMessage = "{0} 字段是必填的")]
        [MaxLength(50, ErrorMessage = "{0} 的最大长度为 {1}。")]
        public string ReceiverName { get; set; }
        [Display(Name = "电话号码")]
        [Required(ErrorMessage = "{0} 字段是必填的")]
        [MaxLength(50, ErrorMessage = "{0} 的最大长度为 {1}。")]
        public string PhoneNumber { get; set; }
        [Display(Name = "邮政编码")]
        [Required(ErrorMessage = "{0} 字段是必填的")]
        [MaxLength(6, ErrorMessage = "{0} 的最大长度为 {1}。")]
        public string PostCode { get; set; }
        [Display(Name = "排序ID")]
        [Required(ErrorMessage = "{0} 字段是必填的")]
        public int OrderById { get; set; }
        [Display(Name = "是否删除")]
        public bool IsDeleted { get; set; } = false;
        [Display(Name = "是否默认地址")]
        public bool IsDefault { get; set; } = false;
    }
}
