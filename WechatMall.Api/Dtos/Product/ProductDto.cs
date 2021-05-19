namespace WechatMall.Api.Dtos
{
    public class ProductDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int StockCount { get; set; }
        public int SoldCount { get; set; }
        public decimal Price { get; set; }
        public string ShippingAddress { get; set; }
        public int ShippingFareID { get; set; }
        public int OrderbyId { get; set; }
    }
}
