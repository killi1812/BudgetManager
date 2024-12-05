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
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username));
        CreateMap<UserDto, User>();

        CreateMap<RegisterVM, NewUserDto>()
            .ForMember(dest => dest.Admin, opt => opt.MapFrom(src => false));
    }
}