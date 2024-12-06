using System;
using System.Collections.Generic;

namespace Data.Models;

public partial class Log
{
    public int Id { get; set; }

    public DateTime Date { get; set; } = DateTime.Now;

    public string Message { get; set; } = null!;

    public int Lvl { get; set; }
}
