using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            CreateMap<AddrUpdateDto, ShippingAddr>();
        }
    }
}
