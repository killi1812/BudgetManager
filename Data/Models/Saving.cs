using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class Saving
{
    public long Idsavings { get; set; }

    public Guid Guid { get; set; } = System.Guid.NewGuid();

    public decimal? Goal { get; set; }

    public decimal? Current { get; set; }

    public DateTime? Date { get; set; }

    public long? UserId { get; set; }

    public virtual User? User { get; set; }
}
