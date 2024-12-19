using AutoMapper;
using Data.Models;
using Data.Services;
using Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers;

[Authorize]
public class BudgetController : Controller
{
    private readonly IBudgetService _budgetService;
    private readonly IMapper _mapper;

    public BudgetController(IBudgetService budgetService, IMapper mapper)
    {
        _budgetService = budgetService;
        _mapper = mapper;
    }

    public async Task<IActionResult> Budgets()
    {
        var guid = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserGuid")?.Value;
        if (guid == null)
            return BadRequest("User not found");

        var budgets = await _budgetService.GetAll(Guid.Parse(guid));
        var vm = _mapper.Map<List<BudgetVM>>(budgets);
        return View(vm);
    }

    public async Task<IActionResult> EditBudget(string guid)
    {
        var budget = await _budgetService.Get(Guid.Parse(guid));
        var vm = _mapper.Map<BudgetVM>(budget);
        return View(vm);
    }

    public IActionResult CreateBudget()
    {
        return View();
    }

    public async Task<IActionResult> CreateBudgetAction(BudgetVM newBudget)
    {
        var budget = _mapper.Map<Budget>(newBudget);
        await _budgetService.Create(budget);
        return Redirect(nameof(Budgets));
    }

    public async Task<IActionResult> EditBudgetAction(BudgetVM updatedBudget)
    {
        var budget = _mapper.Map<Budget>(updatedBudget);
        budget.Guid = Guid.Parse(updatedBudget.Guid.ToString());
        await _budgetService.Edit(budget);
        return Redirect(nameof(Budgets));
    }

    public async Task<IActionResult> DeleteBudget(string guid)
    {
        await _budgetService.Delete(Guid.Parse(guid));
        return Redirect(nameof(Budgets));
    }

    public async Task<IActionResult> DetailsBudget(string guid)
    {
        if (string.IsNullOrEmpty(guid))
            return BadRequest("Budget GUID is required");

        var budget = await _budgetService.Get(Guid.Parse(guid));
        if (budget == null)
            return NotFound($"Budget with GUID {guid} not found");

        var vm = _mapper.Map<BudgetVM>(budget);

        return View(vm);
    }
}
