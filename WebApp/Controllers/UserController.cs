using AutoMapper;
using Data.Dto;
using Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers;

[Authorize]
public class UserController : Controller
{
    private readonly IUserManagementService _userManagementService;
    private readonly IProfileService _profileService;
    private readonly IMapper _mapper;

    public UserController(IUserManagementService userManagementService, IProfileService profileService, IMapper mapper)
    {
        _userManagementService = userManagementService;
        _profileService = profileService;
        _mapper = mapper;
    }

    [Authorize]
    public async Task<IActionResult> Account(string guid)
    {
        if (string.IsNullOrEmpty(guid))
        {
            guid = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserGuid")?.Value;
            if (guid == null)
                return BadRequest("User not found");
        }

        var user = await _userManagementService.GetUser(Guid.Parse(guid));
        var userDto = _mapper.Map<UserDto>(user);
        var userVm = _mapper.Map<UserVM>(userDto);
        return View(userVm);
    }

    [Authorize]
    public async Task<IActionResult> EditUser([FromBody] UserDto dto)
    {
        var userGuid = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserGuid")?.Value);
        await _userManagementService.EditUser(userGuid, dto);
        return Ok();
    }

    [Authorize]
    public async Task<IActionResult> UpdateProfilePicture(IFormFile profilePicture)
    {
        var userGuid = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserGuid")?.Value);

        if (profilePicture == null || profilePicture.Length == 0)
            return BadRequest("Invalid file");

        var filePath = Path.Combine("wwwroot", "images", "profile_pictures", $"{userGuid}_{profilePicture.FileName}");
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await profilePicture.CopyToAsync(stream);
        }

        await _profileService.UpdateProfilePicture(userGuid, $"/images/profile_pictures/{userGuid}_{profilePicture.FileName}");
        return RedirectToAction("Account");
    }
}
