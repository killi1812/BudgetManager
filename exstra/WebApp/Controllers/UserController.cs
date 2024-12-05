using AutoMapper;
using Data.Dto;
using Data.Models;
using Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers;

public class UserController : Controller
{
    private readonly IMapper _mapper;
    private readonly IUserServices _userServices;

    public UserController( IUserServices userServices, IMapper mapper)
    {
        _userServices = userServices;
        _mapper = mapper;
    }

    [Authorize]
    public async Task<IActionResult> Account(string guid)
    {
        if (guid == null)
        {
            guid = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserGuid")?.Value;
            if (guid == null)
                return BadRequest("User not found");
        }

        var user = await _userServices.GetUser(Guid.Parse(guid));
        var userVm = _mapper.Map<UserVM>(user);
        return View(userVm);
    }

    [Authorize]
    public async Task<IActionResult> ChangePassword(string oldPassword, string newPassword)
    {
        var userGuid = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserGuid")?.Value);
        await _userServices.ChangePassword(userGuid, oldPassword, newPassword);
        return Ok();
    }

    [Authorize]
    public async Task<IActionResult> EditUser([FromBody] UserDto dto)
    {
        var userGuid = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserGuid")?.Value);
        var user = _mapper.Map<User>(dto);
        await _userServices.EditUser(userGuid, user);
        var newUser = _mapper.Map<UserVM>(await _userServices.GetUser(userGuid));
        return Ok(newUser);
    }

    [Authorize]
    public async Task<IActionResult> DeleteUser(string guid)
    {
        var userGuid = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserGuid")?.Value);
        await _userServices.DeleteUser(userGuid, Guid.Parse(guid));
        return RedirectToAction("Users", "User");
    }

    [Authorize]
    public async Task<IActionResult> Users()
    {
        var userGuid = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserGuid")?.Value);
        var users = await _userServices.GetUsers(userGuid);
        var usersVm = _mapper.Map<List<UserVM>>(users);
        return View(usersVm);
    }
}