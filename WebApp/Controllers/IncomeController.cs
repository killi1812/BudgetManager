using AutoMapper;
using Data.Models;
using Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers;

[Authorize]
public class IncomeController : Controller
{
    private readonly IIncomeService _incomeService;
    private readonly IMapper _mapper;
    private readonly IUserServices _userServices;

    public IncomeController(IIncomeService incomeService, IMapper mapper, IUserServices userServices)
    {
        _incomeService = incomeService;
        _mapper = mapper;
        _userServices = userServices;
    }

    public async Task<IActionResult> Incomes()
    {
        var incomes = await _incomeService.GetAll();
        var vms = _mapper.Map<List<IncomeVM>>(incomes);
        return View(vms);
    }

    public async Task<IActionResult> EditIncome(string guid)
    {
        var income = await _incomeService.Get(Guid.Parse(guid));
        var vm = _mapper.Map<IncomeVM>(income);
        return View(vm);
    }

    public IActionResult CreateIncome()
    {
        return View();
    }

    public async Task<IActionResult> CreateIncomeAction(IncomeVM incomeVm)
    {
        var newIncome = _mapper.Map<Income>(incomeVm);
        
        var guid = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserGuid")?.Value;
        if (guid == null)
            return BadRequest("User not found");
        var user =  await _userServices.GetUser(Guid.Parse(guid));
        newIncome.UserId = user.Iduser;
        
        await _incomeService.Create(newIncome);
        return Redirect(nameof(Incomes));
    }

    public async Task<IActionResult> EditIncomeAction(IncomeVM incomeVm)
    {
        var incomeUpdate = _mapper.Map<Income>(incomeVm);
        await _incomeService.Update(Guid.Parse(incomeVm.Guid), incomeUpdate);
        return Redirect(nameof(Incomes));
    }

    public async Task<IActionResult> DeleteIncome(string guid)
    {
        await _incomeService.Delete(Guid.Parse(guid));
        return Redirect(nameof(Incomes));
    }
}