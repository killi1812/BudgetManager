using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class Statistic
{
    public long Idstatistics { get; set; }

    public Guid Guid { get; set; } = System.Guid.NewGuid();

    public decimal? TotalSpent { get; set; }

    public decimal? TotalIncome { get; set; }

    public decimal? SpendingPercent { get; set; }

    public decimal? IncomePercent { get; set; }

    public long? UserId { get; set; }

    public virtual User? User { get; set; }
}
