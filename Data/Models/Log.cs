namespace Data.Models;

public class Log
{
    public int Id { get; set; }

    public DateTime Date { get; set; } = DateTime.Now;

    public string Message { get; set; } = null!;

    public int Lvl { get; set; }
}
