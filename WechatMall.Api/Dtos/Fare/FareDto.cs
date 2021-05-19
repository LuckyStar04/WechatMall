using System;

namespace WechatMall.Api.Dtos
{
    public class FareDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime EditTime { get; set; }
        public string Rules { get; set; }
        public bool IsDeleted { get; set; }
    }
}
