using Data.Helpers;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Services
{
    public class ExpenseService
    {
        private readonly BudgetManagerContext _context;

        public ExpenseService(BudgetManagerContext context)
        {
            _context = context;
        }

        public async Task<List<Expense>> GetExpensesByStatus(Guid userGuid, string status)
        {
            return await _context.Expenses
                .Where(e => e.User.Guid == userGuid && e.Status == status)
                .OrderBy(e => e.Date)
                .Include(e => e.Category)
                .AsNoTracking()
                .ToListAsync();
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
}
