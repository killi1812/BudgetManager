using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class Budget
{
    public long Idbudget { get; set; }

    public Guid Guid { get; set; } = System.Guid.NewGuid();

    public decimal? Sum { get; set; }

    public long? UserId { get; set; }

    public long? CategoryId { get; set; }

    public virtual Category? Category { get; set; }

    public virtual User? User { get; set; }
}
