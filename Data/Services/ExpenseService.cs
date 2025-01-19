using Data.Helpers;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public interface IExpenseService
    {
        Task<Expense> Create(Expense newExpense);
        Task<Expense> Get(Guid guid);
        Task Delete(Guid guid);
        Task<List<Expense>> GetAll(Guid userGuid);
        Task<Expense> Edit(Guid guid, Expense updatedExpense);
    }

    public class ExpenseService : IExpenseService
    {
        private readonly BudgetManagerContext _context;

        public ExpenseService(BudgetManagerContext context)
        {
            _context = context;
        }

        public async Task<Expense> Create(Expense newExpense)
        {
            await _context.Expenses.AddAsync(newExpense);
            await _context.SaveChangesAsync();
            return await _context.Expenses.FirstOrDefaultAsync(e => e.Guid == newExpense.Guid);
        }

        public async Task<Expense> Get(Guid guid)
        {
            var expense = await _context.Expenses
        .Include(e => e.Category) 
        .Include(e => e.User)     
        .FirstOrDefaultAsync(e => e.Guid == guid);

            if (expense == null)
                throw new NotFoundException($"Expense with guid: {guid} not found");

            if (expense.Category == null)
                throw new NotFoundException($"Category associated with expense {guid} not found");

            return expense;
        }

        public async Task Delete(Guid guid)
        {
            var expense = await _context.Expenses.FirstOrDefaultAsync(e => e.Guid == guid);
            if (expense == null) throw new NotFoundException($"Expense with guid: {guid} not found");
            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Expense>> GetAll(Guid userGuid)
        {
            var expenses = await _context.Expenses
                .Where(e => e.User.Guid == userGuid)
                .Include(e => e.Category)
                .AsNoTracking()
                .ToListAsync();
            return expenses;
        }

        public async Task<Expense> Edit(Guid guid, Expense updatedExpense)
        {
            var expense = await _context.Expenses.FirstOrDefaultAsync(e => e.Guid == guid);
            if (expense == null) throw new NotFoundException($"Expense with guid: {updatedExpense.Guid} not found");

            expense.Sum = updatedExpense.Sum;
            expense.Description = updatedExpense.Description;
            expense.Date = updatedExpense.Date;
            expense.CategoryId = updatedExpense.CategoryId;
            expense.UserId = updatedExpense.UserId;

            await _context.SaveChangesAsync();
            return await _context.Expenses.AsNoTracking().FirstOrDefaultAsync(e => e.Guid == guid)
                   ?? throw new NotFoundException($"Expense with guid: {updatedExpense.Guid} not found");
        }
    }

}
