namespace WebApp.ViewModels;

public class UserVM
{
    public string Guid { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? ProfilePictureUrl { get; set; } = "/images/default.png";

    public string FullName => $"{FirstName} {LastName}"; // Add FullName as a computed property
}