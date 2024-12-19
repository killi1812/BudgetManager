using AutoMapper;
using Data.Helpers;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Services;

public interface IIncomeService
{
    public Task<List<Income>> GetAll(Guid guid);
    public Task<Income> Create(Income newIncome);
    public Task<Income> Get(Guid guid);
    public Task Delete(Guid guid);
    public Task<Income> Update(Guid guid, Income newIncome);
}

public class IncomeService : IIncomeService
{
    private readonly BudgetManagerContext _context;
    private readonly IMapper _mapper;

    public IncomeService(BudgetManagerContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<Income>> GetAll(Guid guid)
    {
        var incomes =await _context.Incomes.AsNoTracking().Where(i => i.User != null && i.User.Guid == guid).ToListAsync();
        return incomes;
    }

    public async Task<Income> Create(Income newIncome)
    {
        await _context.Incomes.AddAsync(newIncome);
        await _context.SaveChangesAsync();
        return await _context.Incomes
                   .AsNoTracking()
                   .FirstOrDefaultAsync(i => i.Guid == newIncome.Guid) ??
               throw new NotFoundException($"Income with guid: {newIncome.Guid} not found");
    }

    public async Task<Income> Get(Guid guid)
    {
        var income = await _context.Incomes
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Guid == guid);
        if (income == null)
            throw new NotFoundException($"Income with guid: {guid} not found");
        return income;
    }

    public async Task Delete(Guid guid)
    {
        var income = await _context.Incomes
            .FirstOrDefaultAsync(i => i.Guid == guid);
        if (income == null)
            throw new NotFoundException($"Income with guid: {guid} not found");
        _context.Remove(income);
        await _context.SaveChangesAsync();
    }

    public async Task<Income> Update(Guid guid, Income newIncome)
    {
        var income = await _context.Incomes
            .FirstOrDefaultAsync(i => i.Guid == guid);
        if (income == null)
            throw new NotFoundException($"Income with guid: {guid} not found");
        _mapper.Map(newIncome, income);
        await _context.SaveChangesAsync();
        return (await _context.Incomes
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Guid == guid))!;
    }
}