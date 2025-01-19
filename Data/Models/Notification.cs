using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class Notification
{
    public long Idbudget { get; set; }

    public Guid Guid { get; set; } = System.Guid.NewGuid();

    public string Message { get; set; } = null!;

    public bool Read { get; set; } = false;

    public long? UserId { get; set; }

    public virtual User? User { get; set; }
}
