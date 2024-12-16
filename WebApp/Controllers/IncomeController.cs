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

    public IncomeController(IIncomeService incomeService, IMapper mapper)
    {
        _incomeService = incomeService;
        _mapper = mapper;
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
        //TODO take user that creates request
        //newIncome.UserId = 1;
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