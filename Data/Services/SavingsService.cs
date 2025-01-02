using Data.Helpers;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Services;

public interface ISavingService
{
    Task<Saving> Create(Saving newSaving);
    Task<Saving> Get(Guid guid);
    Task Delete(Guid guid);
    Task<List<Saving>> GetAll(Guid userGuid);
    Task<Saving> Edit(Saving updatedSaving);
}

public class SavingService : ISavingService
{
    private readonly BudgetManagerContext _context;

    public SavingService(BudgetManagerContext context)
    {
        _context = context;
    }

    public async Task<Saving> Create(Saving newSaving)
    {
        await _context.Savings.AddAsync(newSaving);
        await _context.SaveChangesAsync();
        return await _context.Savings.FirstOrDefaultAsync(s => s.Guid == newSaving.Guid);
    }

    public async Task<Saving> Get(Guid guid)
    {
        var saving = await _context.Savings
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.Guid == guid);
        if (saving == null) throw new NotFoundException($"Saving with guid: {guid} not found");
        return saving;
    }

    public async Task Delete(Guid guid)
    {
        var saving = await _context.Savings.FirstOrDefaultAsync(s => s.Guid == guid);
        if (saving == null) throw new NotFoundException($"Saving with guid: {guid} not found");
        _context.Savings.Remove(saving);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Saving>> GetAll(Guid userGuid)
    {
        var savings = await _context.Savings
            .Where(s => s.User.Guid == userGuid)
            .AsNoTracking()
            .ToListAsync();
        return savings;
    }

    public async Task<Saving> Edit(Saving updatedSaving)
    {
        var saving = await _context.Savings.FirstOrDefaultAsync(s => s.Guid == updatedSaving.Guid);
        if (saving == null) throw new NotFoundException($"Saving with guid: {updatedSaving.Guid} not found");

        saving.Goal = updatedSaving.Goal;
        saving.Current = updatedSaving.Current;
        saving.UserId = updatedSaving.UserId;

        await _context.SaveChangesAsync();
        return await _context.Savings.AsNoTracking().FirstOrDefaultAsync(s => s.Guid == updatedSaving.Guid)
               ?? throw new NotFoundException($"Saving with guid: {updatedSaving.Guid} not found");
    }
}
