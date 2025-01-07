namespace Data.Dto;

public class NewUserDto
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Jmbag { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? ProfilePictureUrl { get; set; } = "/images/default.png";
    public long? RoleId { get; set; }
    public string Username { get; set; }
}