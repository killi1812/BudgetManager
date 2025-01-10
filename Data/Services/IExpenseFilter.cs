using Data.Models;

namespace Data.Services;

public interface IExpenseFilter
{
    IQueryable<Expense> Filter(IQueryable<Expense> expenses);
}
