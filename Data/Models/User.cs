namespace Data.Models;

public class User
{
    public int Id { get; set; }

    public Guid Guid { get; set; } = Guid.NewGuid();

    public string Username { get; set; } = null!;

    public bool Admin { get; set; }

    public string Password { get; set; } = null!;
}
