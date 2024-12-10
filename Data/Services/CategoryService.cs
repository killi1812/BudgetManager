using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Services;

public interface ICategoryService
{
    Task<Category> Create(Category newCategory);
    Task<Category> Get(Guid guid);
    Task Delete(Guid guid);
    Task<Category> Create(Guid guid,Category newCategory);
    Task<List<Category>> GetAll();
}

public class CategoryService : ICategoryService
{
    private readonly BudgetManagerContext _context;

    public CategoryService(BudgetManagerContext context)
    {
        _context = context;
    }

    public async Task<Category> Create(Category newCategory)
    {
        await _context.Categories.AddAsync(newCategory);
        await _context.SaveChangesAsync();
        return await _context.Categories.FirstOrDefaultAsync(c => c.Guid == newCategory.Guid);
    }

    public Task<Category> Get(Guid guid)
    {
        throw new NotImplementedException();
    }

    public Task Delete(Guid guid)
    {
        throw new NotImplementedException();
    }

    public Task<Category> Create(Guid guid, Category newCategory)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Category>> GetAll()
    {
       var cats   = await _context.Categories.AsNoTracking().ToListAsync();
       return cats;
    }
}