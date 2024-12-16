using AutoMapper;
using Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

    public async Task<IActionResult> ListIncome()
    {
        var incomes = await _incomeService.GetAll();
       return View()
    }
}