using AutoMapper;
using Data.Models;
using Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Helpers;
using WebApp.ViewModels;

namespace WebApp.Controllers;

[Authorize]
public class BudgetController : Controller
{
    private readonly IBudgetService _budgetService;
    private readonly IUserServices _userServices;
    private readonly ICategoryService _categoryService;
    private readonly IMapper _mapper;

    public BudgetController(IBudgetService budgetService, IMapper mapper, ICategoryService categoryService,
        IUserServices userServices)
    {
        _budgetService = budgetService;
        _mapper = mapper;
        _categoryService = categoryService;
        _userServices = userServices;
    }

    public async Task<IActionResult> Budgets()
    {
        var guid = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserGuid")?.Value;
        if (guid == null)
            return BadRequest("Guid can't be null");

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

        budget.Category = await _categoryService.Get(Guid.Parse(newBudget.CategoryGuid));
        budget.User = await _userServices.GetUser(HttpContext.GetUserGuid());

        await _budgetService.Create(budget);
        return Redirect(nameof(Budgets));
    }

    public async Task<IActionResult> EditBudgetAction(BudgetVM updatedBudget)
    {
        var budget = _mapper.Map<Budget>(updatedBudget);
        
        budget.Category = await _categoryService.Get(Guid.Parse(updatedBudget.CategoryGuid));
        budget.CategoryId = budget.Category.Idcategory;
        budget.User = await _userServices.GetUser(HttpContext.GetUserGuid());
        budget.UserId = budget.User.Iduser;
        
        await _budgetService.Edit(Guid.Parse(updatedBudget.Guid), budget);
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
        var vm = _mapper.Map<BudgetVM>(budget);
        return View(vm);
    }
}