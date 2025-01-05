using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class Expense
{
    public long Idexpense { get; set; }

    public Guid Guid { get; set; } = System.Guid.NewGuid();

    public decimal? Sum { get; set; }

    public string? Description { get; set; }

    public DateTime? Date { get; set; }

    public long? CategoryId { get; set; }

    public long? UserId { get; set; }

    public long? PayerId { get; set; }

    public string? Status { get; set; }

    public virtual Category? Category { get; set; }

    public virtual User? Payer { get; set; }

    public virtual User? User { get; set; }
}
