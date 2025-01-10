using Data.Models;

namespace Data.Services;

public class UnpaidExpenseFilter : IExpenseFilter
{
    public IQueryable<Expense> Filter(IQueryable<Expense> expenses)
    {
        return expenses.Where(e => e.Status == "Unpaid");
    }
}
