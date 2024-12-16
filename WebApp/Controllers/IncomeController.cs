using AutoMapper;
using Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers;

[Authorize]
public class IncomeController: Controller
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

    public IActionResult EditIncome(Guid guid)
    {
        throw new NotImplementedException();
    }

    public IActionResult CreateIncome()
    {
        throw new NotImplementedException();
    }
}