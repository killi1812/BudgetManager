using AutoMapper;
using Data.Models;
using Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Helpers;
using WebApp.ViewModels;

namespace WebApp.Controllers;

[Authorize]
public class SavingsController : Controller
{
    private readonly ISavingService _savingService;
    private readonly IUserManagementService _userServices;
    private readonly IMapper _mapper;

    public SavingsController(ISavingService savingService, IMapper mapper, IUserManagementService userServices)
    {
        _savingService = savingService;
        _mapper = mapper;
        _userServices = userServices;
    }

    public async Task<IActionResult> Savings()
    {
        var guid = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserGuid")?.Value;
        if (guid == null)
            return BadRequest("User not found");

        var savings = await _savingService.GetAll(Guid.Parse(guid));
        var vm = _mapper.Map<List<SavingsVM>>(savings);
        return View(vm);
    }

    public async Task<IActionResult> EditSaving(string guid)
    {
        if (string.IsNullOrEmpty(guid))
            return BadRequest("GUID is required");

        var saving = await _savingService.Get(Guid.Parse(guid));

        var vm = _mapper.Map<SavingsVM>(saving);
        return View(vm);
    }

    public IActionResult CreateSaving()
    {
        return View();
    }

    public async Task<IActionResult> CreateSavingsAction(SavingsVM newSaving)
    {
        newSaving.Date = DateTime.Now;
        var saving = _mapper.Map<Saving>(newSaving);

        saving.User = await _userServices.GetUser(HttpContext.GetUserGuid());
        saving.UserId = saving.User.Iduser;

        await _savingService.Create(saving);
        return Redirect(nameof(Savings));
    }

    public async Task<IActionResult> EditSavingsAction(SavingsVM updatedSaving)
    {
        var saving = _mapper.Map<Saving>(updatedSaving);

        saving.User = await _userServices.GetUser(HttpContext.GetUserGuid());
        saving.UserId = saving.User.Iduser;

        saving.Guid = Guid.Parse(updatedSaving.Guid);
        await _savingService.Edit(saving);
        return Redirect(nameof(Savings));
    }

    public async Task<IActionResult> DeleteSavingsAction(string guid)
    {
        if (String.IsNullOrEmpty(guid))
        {
            return BadRequest("Invalid saving data");
        }

        await _savingService.Delete(Guid.Parse(guid));
        return RedirectToAction(nameof(Savings));
    }

    public async Task<IActionResult> DetailsSaving(string guid)
    {
        if (string.IsNullOrEmpty(guid))
            return BadRequest("Saving GUID is required");

        var saving = await _savingService.Get(Guid.Parse(guid));
        var vm = _mapper.Map<SavingsVM>(saving);

        return View(vm);
    }
}