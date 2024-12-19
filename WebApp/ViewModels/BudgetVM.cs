using Data.Models;
using System;

namespace Data.ViewModels
{
    public class BudgetVM
    {
        public long Id { get; set; }

        public decimal? Sum { get; set; }

        public long? UserId { get; set; }

        public long? CategoryId { get; set; }

        public Guid Guid { get; set; }

        public string? CategoryName { get; set; } 
        public string? UserName { get; set; }     

        public BudgetVM() { }

        public BudgetVM(long id, decimal? sum, long? userId, long? categoryId, Guid guid, string? categoryName = null, string? userName = null)
        {
            Id = id;
            Sum = sum;
            UserId = userId;
            CategoryId = categoryId;
            Guid = guid;
            CategoryName = categoryName;
            UserName = userName;
        }

        public static BudgetVM FromBudget(Budget budget)
        {
            return new BudgetVM
            {
                Id = budget.Idbudget,
                Sum = budget.Sum,
                UserId = budget.UserId,
                CategoryId = budget.CategoryId,
                Guid = budget.Guid,
                CategoryName = budget.Category?.CategoryName, 
                UserName = budget.User?.Jmbag
            };
        }
    }
}
