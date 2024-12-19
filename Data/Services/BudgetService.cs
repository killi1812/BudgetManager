using Data.Helpers;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Services;

public interface IBudgetService
{
    Task<Budget> Create(Budget newBudget);
    Task<Budget> Get(Guid guid);
    Task Delete(Guid guid);
    Task<List<Budget>> GetAll(Guid userGuid);
    Task<Budget> Edit(Budget updatedBudget);
}

public class BudgetService : IBudgetService
{
    private readonly BudgetManagerContext _context;

    public BudgetService(BudgetManagerContext context)
    {
        _context = context;
    }

    public async Task<Budget> Create(Budget newBudget)
    {
        await _context.Budgets.AddAsync(newBudget);
        await _context.SaveChangesAsync();
        return await _context.Budgets.FirstOrDefaultAsync(b => b.Guid == newBudget.Guid);
    }

    public async Task<Budget> Get(Guid guid)
    {
        var budget = await _context.Budgets
            .Include(b => b.Category)
            .Include(b => b.User)
            .FirstOrDefaultAsync(b => b.Guid == guid);
        if (budget == null) throw new NotFoundException($"Budget with guid: {guid} not found");
        return budget;
    }

    public async Task Delete(Guid guid)
    {
        var budget = await _context.Budgets.FirstOrDefaultAsync(b => b.Guid == guid);
        if (budget == null) throw new NotFoundException($"Budget with guid: {guid} not found");
        _context.Budgets.Remove(budget);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Budget>> GetAll(Guid userGuid)
    {
        var budgets = await _context.Budgets
            .Where(b => b.User.Guid == userGuid)
            .Include(b => b.Category)
            .AsNoTracking()
            .ToListAsync();
        return budgets;
    }

    public async Task<Budget> Edit(Budget updatedBudget)
    {
        var budget = await _context.Budgets.FirstOrDefaultAsync(b => b.Guid == updatedBudget.Guid);
        if (budget == null) throw new NotFoundException($"Budget with guid: {updatedBudget.Guid} not found");

        budget.Sum = updatedBudget.Sum;
        budget.UserId = updatedBudget.UserId;
        budget.CategoryId = updatedBudget.CategoryId;

        await _context.SaveChangesAsync();
        return await _context.Budgets.AsNoTracking().FirstOrDefaultAsync(b => b.Guid == updatedBudget.Guid)
               ?? throw new NotFoundException($"Budget with guid: {updatedBudget.Guid} not found");
    }
}
