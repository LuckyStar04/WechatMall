using AutoMapper;
using System.Linq;
using WechatMall.Api.Dtos;
using WechatMall.Api.Entities;

namespace WechatMall.Api.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => src.ProductID))
                .ForMember(
                    dest => dest.Image,
                    opt => opt.MapFrom(src => src.Images.Count == 0 ? "" : src.Images[0].ImagePath)
                );
            CreateMap<Product, ProductDetailDto>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => src.ProductID))
                .ForMember(
                    dest => dest.Image,
                    opt => opt.MapFrom(src => src.Images.Select(i => i.ImagePath))
                );
            CreateMap<ProductImage, ProductImageDto>();
        }
    }
}
