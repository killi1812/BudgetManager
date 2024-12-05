namespace WebApp.ViewModels;

public class UserVM
{
    public string Guid { get; set; } = null!;
    public string Username { get; set; } = null!;
    public bool Admin { get; set; } = false;
    public List<string> Downloads { get; set; } = new();
}