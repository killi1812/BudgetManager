using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class BankAccountApi
{
    public long IdbankAccountApi { get; set; }

    public Guid Guid { get; set; }

    public string? BankName { get; set; }

    public decimal? Balance { get; set; }

    public string? Url { get; set; }

    public string? Apikey { get; set; }

    public long? UserId { get; set; }

    public virtual User? User { get; set; }
}
