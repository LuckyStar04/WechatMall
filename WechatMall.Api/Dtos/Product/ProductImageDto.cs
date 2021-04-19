using System;

namespace WechatMall.Api.Dtos
{
    public class ProductImageDto
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string ProductID { get; set; }
        public string ImagePath { get; set; }
        public int OrderbyId { get; set; }
    }
}
