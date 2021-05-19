using System.Collections.Generic;
using WechatMall.Api.Entities;

namespace WechatMall.Api.Dtos
{
    public class ProductDetailDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Image { get; set; } = new List<string>();
        public int StockCount { get; set; }
        public int SoldCount { get; set; }
        public decimal Price { get; set; }
        public string ShippingAddress { get; set; }
        public int ShippingFareID { get; set; }
        public string Detail { get; set; }
        public int OrderbyId { get; set; }
    }
}
