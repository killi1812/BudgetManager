using System.Security.Claims;
using AutoMapper;
using Data.Dto;
using Data.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers;

public class AuthController : Controller
{
    private readonly IUserServices _userServices;
    private readonly IMapper _mapper;

    public AuthController(IUserServices userServices, IMapper mapper)
    {
        _userServices = userServices;
        _mapper = mapper;
    }

    public IActionResult Login(string returnUrl = "/Home/Index")
    {
        return View(new LoginVM { ReturnUrl = returnUrl });
    }

    public async Task<IActionResult> LoginAction(LoginVM loginVm)
    {
        var claims = await _userServices.LoginCookie(loginVm.Username, loginVm.Password);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claims.claimsIdentity), claims.authProperties);

        HttpContext.Session.SetString("username", loginVm.Username);

        return Redirect(loginVm.ReturnUrl);
    }

    public IActionResult Register()
    {
        return View();
    }

    public async Task<IActionResult> RegisterAction(RegisterVM vm)
    {
        if (!ModelState.IsValid)
        {
            return View("Register", vm);
        }

        var newUserDto = _mapper.Map<NewUserDto>(vm);
        await _userServices.CreateUser(newUserDto);
        return Redirect(nameof(Login));
    }




    public IActionResult Logout(string redirectUrl = "/Home/Index")
    {
        HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Redirect(redirectUrl);
    }

    public IActionResult AccessDenied()
    {
        return View();
    }
}