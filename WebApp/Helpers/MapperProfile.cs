using AutoMapper;
using Data.Dto;
using Data.Models;
using WebApp.ViewModels;

namespace WebApp.Helpers;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
           CreateMap<User, UserVM>()
            .ForMember(dest => dest.Guid, opt => opt.MapFrom(src => src.Guid.ToString()))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Email));
        CreateMap<UserDto, User>();
        CreateMap<RegisterVM, NewUserDto>();
        CreateMap<NewUserDto, User>();
        CreateMap<RegisterVM, NewUserDto>();

        CreateMap<CategoryVM, Category>()
            .ForMember(dest => dest.Guid, opt => opt.Ignore())
            .ForMember(dest => dest.Budgets, opt => opt.Ignore())
            .ForMember(dest => dest.Expenses, opt => opt.Ignore())
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Name));
        CreateMap<Category, CategoryVM>()
            .ForMember(dest => dest.Guid, opt => opt.MapFrom(src => src.Guid.ToString()))
            .ForMember(dest => dest.Name,opt => opt.MapFrom(src => src.CategoryName));

    }
}