using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WechatMall.Api.Entities
{
    public class SiteConfig
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(255)]
        public string Key { get; set; }
        [Required, StringLength(4096)]
        public string Value { get; set; }
    }
}
