using AutoMapper;
using WechatMall.Api.Dtos;
using WechatMall.Api.Entities;

namespace WechatMall.Api.Profiles
{
    public class FareProfile : Profile
    {
        public FareProfile()
        {
            CreateMap<ShippingFare, FareDto>();
            CreateMap<ShippingFare, FareUpdateDto>();
            CreateMap<FareUpdateDto, ShippingFare>();
            CreateMap<FareAddDto, ShippingFare>();
        }
    }
}
