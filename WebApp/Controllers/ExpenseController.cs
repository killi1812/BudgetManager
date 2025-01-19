using AutoMapper;
using Data.Models;
using Data.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Helpers;
using WebApp.ViewModels;
using Microsoft.Extensions.Logging;

namespace WebApp.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly IExpenseService _expenseService;
        private readonly IUserServices _userServices;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly ILogger<ExpenseController> _logger;

        public ExpenseController(IExpenseService expenseService, IMapper mapper, ICategoryService categoryService,
            IUserServices userServices, ILogger<ExpenseController> logger)
        {
            _expenseService = expenseService;
            _mapper = mapper;
            _categoryService = categoryService;
            _userServices = userServices;
            _logger = logger;
        }

        public async Task<IActionResult> Expenses()
        {
            var guid = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserGuid")?.Value;
            if (guid == null)
                return BadRequest("Guid can't be null");

            var expenses = await _expenseService.GetAll(Guid.Parse(guid));
            var vm = _mapper.Map<List<ExpenseVM>>(expenses);
            return View(vm);
        }

        public async Task<IActionResult> EditExpense(string guid)
        {
            if (string.IsNullOrEmpty(guid))
            {
                return BadRequest("Expense GUID is required.");
            }

            if (!Guid.TryParse(guid, out Guid expenseGuid))
            {
                return BadRequest("Invalid expense GUID.");
            }

            var expense = await _expenseService.Get(expenseGuid);
            if (expense == null)
            {
                return NotFound($"Expense with GUID {guid} not found.");
            }

            var vm = _mapper.Map<ExpenseVM>(expense);
            return View(vm);
        }


        public async Task<IActionResult> CreateExpense()
        {
            var categories = await _categoryService.GetAll(Guid.Empty);

            ViewBag.Categories = new SelectList(categories, "Guid", "CategoryName");
            return View();
        }

        public async Task<IActionResult> CreateExpenseAction(ExpenseVM newExpense)
        {
            var expense = _mapper.Map<Expense>(newExpense);

            expense.Category = await _categoryService.Get(Guid.Parse(newExpense.CategoryGuid));
            expense.User = await _userServices.GetUser(HttpContext.GetUserGuid());

            await _expenseService.Create(expense);
            return Redirect(nameof(Expenses));
        }

        public async Task<IActionResult> EditExpenseAction(ExpenseVM updatedExpense)
        {
            if (updatedExpense == null)
            {
                return BadRequest("Invalid expense data.");
            }

            var expense = _mapper.Map<Expense>(updatedExpense);

            expense.Category = await _categoryService.Get(Guid.Parse(updatedExpense.CategoryGuid));
            expense.CategoryId = expense.Category.Idcategory;
            expense.User = await _userServices.GetUser(HttpContext.GetUserGuid());
            expense.UserId = expense.User.Iduser;

            await _expenseService.Edit(Guid.Parse(updatedExpense.Guid), expense);
            return Redirect(nameof(Expenses));
        }

        public async Task<IActionResult> DeleteExpense(string guid)
        {
            await _expenseService.Delete(Guid.Parse(guid));
            return Redirect(nameof(Expenses));
        }

        public async Task<IActionResult> DetailsExpense(string guid)
        {
            if (string.IsNullOrEmpty(guid))
                return BadRequest("Expense GUID is required");

            var expense = await _expenseService.Get(Guid.Parse(guid));
            var vm = _mapper.Map<ExpenseVM>(expense);
            return View(vm);
        }
    }
}
