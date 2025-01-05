using AutoMapper;
using Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    [Authorize]
    public class ExpenseController : Controller
    {
        private readonly ExpenseService _expenseService;
        private readonly IMapper _mapper;

        public ExpenseController(ExpenseService expenseService, IMapper mapper)
        {
            _expenseService = expenseService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Expenses(string status = "Unpaid")
        {
            var guid = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserGuid")?.Value;
            if (guid == null)
                return BadRequest("User not found");

            var expenses = await _expenseService.GetExpensesByStatus(Guid.Parse(guid), status);
            var vm = _mapper.Map<List<ExpenseVM>>(expenses);

            ViewBag.Status = status;
            return View(vm);
        }

        public async Task<IActionResult> MarkAsPaid(string guid)
        {
            if (string.IsNullOrEmpty(guid))
            {
                return BadRequest("Invalid expense GUID.");
            }

            await _expenseService.MarkAsPaid(Guid.Parse(guid));
            return RedirectToAction(nameof(Expenses), new { status = "Unpaid" });
        }
    }
}
