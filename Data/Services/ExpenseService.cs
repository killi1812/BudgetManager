using Data.Helpers;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Services;

public interface IExpenseService
{
    Task<List<Expense>> GetExpenses(Guid userGuid, IExpenseFilter filter);
    Task MarkAsPaid(Guid expenseGuid);
}

public class ExpenseService : IExpenseService
{
    private readonly BudgetManagerContext _context;

    public ExpenseService(BudgetManagerContext context)
    {
        _context = context;
    }

    public async Task<List<Expense>> GetExpenses(Guid userGuid, IExpenseFilter filter)
    {
        var expenses = _context.Expenses
            .Where(e => e.User.Guid == userGuid)
            .Include(e => e.Category)
            .AsNoTracking();

        return await filter.Filter(expenses).OrderBy(e => e.Date).ToListAsync();
    }

    public async Task MarkAsPaid(Guid expenseGuid)
    {
        var expense = await _context.Expenses.FirstOrDefaultAsync(e => e.Guid == expenseGuid);
        if (expense == null)
        {
            throw new NotFoundException($"Expense with guid {expenseGuid} not found");
        }

        expense.Status = "Paid";
        await _context.SaveChangesAsync();
    }
}
