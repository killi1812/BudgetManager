using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class Income
{
    public long Idincome { get; set; }

    public Guid Guid { get; set; } = System.Guid.NewGuid();

    public decimal? Sum { get; set; }

    public string? Source { get; set; }

    public DateTime? Date { get; set; }

    public string? Frequency { get; set; }

    public long? UserId { get; set; }

    public virtual User? User { get; set; }
}
