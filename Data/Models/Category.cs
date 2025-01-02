using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class Category
{
    public long Idcategory { get; set; }

    public Guid Guid { get; set; }

    public string? CategoryName { get; set; }

    public string? Color { get; set; }

    public virtual ICollection<Budget> Budgets { get; set; } = new List<Budget>();

    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();
}
