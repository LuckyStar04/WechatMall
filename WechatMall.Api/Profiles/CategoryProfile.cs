using AutoMapper;
using WechatMall.Api.Dtos;
using WechatMall.Api.Entities;

namespace WechatMall.Api.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => src.CategoryID));
            CreateMap<CategoryAddDto, Category>()
                .ForMember(
                    dest => dest.CategoryID,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember("Id", opt => opt.Ignore());
            CreateMap<CategoryUpdateDto, Category>()
                .ForMember(
                    dest => dest.CategoryID,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember("Id", opt => opt.Ignore());
        }
    }
}
