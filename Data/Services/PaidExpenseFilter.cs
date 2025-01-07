using Data.Models;

namespace Data.Services;

public class PaidExpenseFilter : IExpenseFilter
{
    public IQueryable<Expense> Filter(IQueryable<Expense> expenses)
    {
        return expenses.Where(e => e.Status == "Paid");
    }
}
