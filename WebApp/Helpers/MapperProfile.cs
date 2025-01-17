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
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CategoryName));

        CreateMap<Income, Income>()
            .ForMember(dest => dest.Guid, opt => opt.Ignore())
            .ForMember(dest => dest.Idincome, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore());
        CreateMap<Income, IncomeVM>()
            .ForMember(dest => dest.Guid, opt => opt.MapFrom(src => src.Guid.ToString()));
        CreateMap<IncomeVM, Income>()
            .ForMember(dest => dest.Guid, opt => opt.Ignore())
            .ForMember(dest => dest.Idincome, opt => opt.Ignore());

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

        CreateMap<Expense, ExpenseVM>()
    .ForMember(dest => dest.CategoryGuid, opt => opt.MapFrom(src => src.Category.Guid.ToString()))
    .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
    .ForMember(dest => dest.UserGuid, opt => opt.MapFrom(src => src.User.Guid.ToString()))
    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"));

        CreateMap<ExpenseVM, Expense>()
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.Guid, opt => opt.Ignore());
        CreateMap<Notification, NotificationVM>();
    }
}