using System;

namespace WechatMall.Api.Dtos
{
    public class FareUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Rules { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
