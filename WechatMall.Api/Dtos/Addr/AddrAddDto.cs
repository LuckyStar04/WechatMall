using System.ComponentModel.DataAnnotations;

namespace WechatMall.Api.Dtos
{
    public class AddrAddDto
    {
        [Required(ErrorMessage = "{0} 字段是必填的")]
        [MaxLength(20, ErrorMessage = "{0} 的最大长度为 {1}。")]
        public string Province { get; set; }
        [Required(ErrorMessage = "{0} 字段是必填的")]
        public int ProvinceID { get; set; }
        [Required(ErrorMessage = "{0} 字段是必填的")]
        [MaxLength(20, ErrorMessage = "{0} 的最大长度为 {1}。")]
        public string City { get; set; }
        [Required(ErrorMessage = "{0} 字段是必填的")]
        public int CityID { get; set; }
        [Required(ErrorMessage = "{0} 字段是必填的")]
        [MaxLength(20, ErrorMessage = "{0} 的最大长度为 {1}。")]
        public string County { get; set; }
        [Required(ErrorMessage = "{0} 字段是必填的")]
        public int CountyID { get; set; }
        [Required(ErrorMessage = "{0} 字段是必填的")]
        [MaxLength(255, ErrorMessage = "{0} 的最大长度为 {1}。")]
        public string Address { get; set; }
        [Required(ErrorMessage = "{0} 字段是必填的")]
        [MaxLength(50, ErrorMessage = "{0} 的最大长度为 {1}。")]
        public string ReceiverName { get; set; }
        [Required(ErrorMessage = "{0} 字段是必填的")]
        [MaxLength(50, ErrorMessage = "{0} 的最大长度为 {1}。")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "{0} 字段是必填的")]
        [MaxLength(6, ErrorMessage = "{0} 的最大长度为 {1}。")]
        public string PostCode { get; set; }
        [Required(ErrorMessage = "{0} 字段是必填的")]
        public int? OrderById { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsDefault { get; set; } = false;
    }
}
