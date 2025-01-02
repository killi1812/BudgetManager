using Data.Helpers;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Services;

public interface ICategoryService
{
    Task<Category> Create(Category newCategory);
    Task<Category> Get(Guid guid);
    Task Delete(Guid guid);
    Task<List<Category>> GetAll(Guid userGuid);
    Task<Category> Edit(Category newCategory);
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

    public async Task<Category> Get(Guid guid)
    {
        var cat = await _context.Categories
            .FirstOrDefaultAsync(c => c.Guid == guid);
        if (cat == null) throw new NotFoundException($"Category with guid: {guid} not found");
        return cat;
    }

    public async Task Delete(Guid guid)
    {
        var cat = await _context.Categories
            .FirstOrDefaultAsync(c => c.Guid == guid);
        if (cat == null) throw new NotFoundException($"Category with guid: {guid} not found");
        _context.Remove(cat);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Category>> GetAll(Guid guid)
    {
        //TODO needs to get only categorys from selected user
        var cats = await _context.Categories
            .AsNoTracking().ToListAsync();
        return cats;
    }

    public async Task<Category> Edit(Category newCategory)
    {
        var cat = await _context.Categories
            .FirstOrDefaultAsync(c => c.Guid == newCategory.Guid);
        if (cat == null) throw new NotFoundException($"Category with guid: {newCategory.Guid} not found");
        cat.CategoryName = newCategory.CategoryName;
        cat.Color = newCategory.Color;
        await _context.SaveChangesAsync();
        return await _context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Guid == newCategory.Guid) ??
               throw new NotFoundException($"Category with guid: {newCategory.Guid} not found");
    }
}