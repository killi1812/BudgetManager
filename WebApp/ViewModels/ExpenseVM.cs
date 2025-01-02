using System;

namespace WebApp.ViewModels
{
    public class ExpenseVM
    {
        public string Guid { get; set; }
        public decimal Sum { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public string? CategoryName { get; set; }
        public string? PayerName { get; set; }
        public string Status { get; set; }
    }
}
