namespace WechatMall.Api.Dtos
{
    public class AddrDto
    {
        public int Id { get; set; }
        public string Province { get; set; }
        public int ProvinceID { get; set; }
        public string City { get; set; }
        public int CityID { get; set; }
        public string County { get; set; }
        public int CountyID { get; set; }
        public string Address { get; set; }
        public string ReceiverName { get; set; }
        public string PhoneNumber { get; set; }
        public string PostCode { get; set; }
        public int OrderById { get; set; }
        public bool IsDefault { get; set; }
        public bool IsDeleted { get; set; }
    }
}
