using System.Collections.Generic;

namespace WebApp.ViewModels
{
    public class BudgetVM
    {
        public long Idbudget { get; set; }

        public decimal? Sum { get; set; }

        public long? UserId { get; set; }

        public long? CategoryId { get; set; }

        public string? CategoryName { get; set; }

        public string? UserName { get; set; }

        public IEnumerable<UserVM>? Users { get; set; }

        public IEnumerable<CategoryVM>? Categories { get; set; }
    }
}
