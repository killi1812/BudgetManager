using System.Collections.Generic;

namespace WebApp.ViewModels
{
    public class BudgetVM
    {
        //GUID je id 
        public string Guid { get; set; }

        public decimal? Sum { get; set; }

        public long? UserId { get; set; }

        public long? CategoryId { get; set; }

        public string? CategoryName { get; set; }

        public string? UserName { get; set; }

        //TODO ovo ostavi za kasnije 
        //public IEnumerable<UserVM>? Users { get; set; }

        //public IEnumerable<CategoryVM>? Categories { get; set; }
    }
}
