using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WechatMall.Api.Dtos;
using WechatMall.Api.Entities;

namespace WechatMall.Api.Profiles
{
    public class CouponProfile : Profile
    {
        public CouponProfile()
        {
            CreateMap<Coupon, CouponDto>();
            CreateMap<CouponAddDto, Coupon>();
            CreateMap<CouponUpdateDto, Coupon>();
            CreateMap<Coupon_User, CouponUserDto>();
            CreateMap<CouponUserUpdateDto, Coupon_User>();
        }
    }
}
