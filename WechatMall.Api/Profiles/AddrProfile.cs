using AutoMapper;
using WechatMall.Api.Dtos;
using WechatMall.Api.Entities;

namespace WechatMall.Api.Profiles
{
    public class AddrProfile : Profile
    {
        public AddrProfile()
        {
            CreateMap<ShippingAddr, AddrDto>();
            CreateMap<AddrAddDto, ShippingAddr>();
            CreateMap<ShippingAddr, AddrUpdateDto>();
            CreateMap<AddrUpdateDto, ShippingAddr>();
        }
    }
}
