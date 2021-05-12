using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WechatMall.Api.Dtos;
using WechatMall.Api.Entities;

namespace WechatMall.Api.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDto>()
                .ForMember(
                    dest => dest.ProductImage,
                    opt => opt.MapFrom(src => src.OrderItems.Count == 0 ? "" : src.OrderItems[0].Product.Images[0].ImagePath)
                )
                .ForMember(
                    dest => dest.OrderItemsCount,
                    opt => opt.MapFrom(src => src.OrderItems.Count)
                );
            CreateMap<Order, OrderDetailDto>();
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(
                    dest => dest.Image,
                    opt => opt.MapFrom(src => src.Product.Images[0].ImagePath)
                )
                .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(src => src.Product.Name)
                );
        }
    }
}
