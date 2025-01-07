using AutoMapper;
using Data.Dto;
using Data.Enums;
using Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.ViewModels;

namespace WebApp.Helpers;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<User, UserVM>()
            .ForMember(dest => dest.Guid, opt => opt.MapFrom(src => src.Guid.ToString()))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));

        CreateMap<UserDto, User>();
        CreateMap<UserDto, UserVM>();
        CreateMap<UserVM, UserDto>();

        CreateMap<User, UserDto>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.ProfilePictureUrl, opt => opt.MapFrom(src => src.ProfilePicture));

        CreateMap<RegisterVM, NewUserDto>();
        CreateMap<NewUserDto, User>();

        CreateMap<Category, CategoryVM>()
            .ForMember(dest => dest.Guid, opt => opt.MapFrom(src => src.Guid.ToString()))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CategoryName));

        CreateMap<CategoryVM, Category>()
            .ForMember(dest => dest.Guid, opt => opt.Ignore())
            .ForMember(dest => dest.Budgets, opt => opt.Ignore())
            .ForMember(dest => dest.Expenses, opt => opt.Ignore())
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Name));

        CreateMap<Income, IncomeVM>()
            .ForMember(dest => dest.Guid, opt => opt.MapFrom(src => src.Guid.ToString()));
        CreateMap<IncomeVM, Income>()
            .ForMember(dest => dest.Guid, opt => opt.Ignore())
            .ForMember(dest => dest.Idincome, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore());

        CreateMap<Budget, BudgetVM>()
            .ForMember(dest => dest.Guid, opt => opt.MapFrom(src => src.Guid.ToString()))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"))
            .ForMember(dest => dest.CategoryGuid, opt => opt.MapFrom(src => src.Category.Guid))
            .ForMember(dest => dest.UserGuid, opt => opt.MapFrom(src => src.User.Guid));

        CreateMap<BudgetVM, Budget>()
            .ForMember(dest => dest.Guid, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.Category, opt => opt.Ignore());

        CreateMap<Saving, SavingsVM>()
            .ForMember(dest => dest.Guid, opt => opt.MapFrom(src => src.Guid.ToString()))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Jmbag));
        CreateMap<SavingsVM, Saving>()
            .ForMember(dest => dest.Guid, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore());


    }
}