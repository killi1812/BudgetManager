using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Services;
public interface IBudgetService
{
       Task<Budget> Create(Budget newCategory);
       Task<Budget> Get(Guid guid);
       Task Delete(Guid guid);
       Task<List<Budget>> GetAll(Guid userGuid);
       Task<Budget> Edit(Budget newCategory); 
}

public class BudgetService: IBudgetService
{
       private readonly BudgetManagerContext _context;

       public BudgetService(BudgetManagerContext context)
       {
              _context = context;
       }

       public Task<Budget> Create(Budget newCategory)
       {
              throw new NotImplementedException();
       }

       public Task<Budget> Get(Guid guid)
       {
              throw new NotImplementedException();
       }

       public Task Delete(Guid guid)
       {
              throw new NotImplementedException();
       }

       public async Task<List<Budget>> GetAll(Guid userGuid)
       {
              var budgets = await _context.Budgets
                     .Where(b => b.User.Guid == userGuid)
                     .AsNoTracking()
                     .ToListAsync();
              return budgets;
       }

       public Task<Budget> Edit(Budget newCategory)
       {
              throw new NotImplementedException();
       }
}