using System;

namespace WechatMall.Api.Dtos
{
    public class FareAddDto
    {
        public string Name { get; set; }
        public string Rules { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
