using System.ComponentModel.DataAnnotations;

namespace WechatMall.Api.Entities
{
    public class SiteConfig
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(64)]
        public string Key { get; set; }
        [Required, StringLength(4096)]
        public string Value { get; set; }
    }
}
