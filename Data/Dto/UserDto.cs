namespace Data.Dto;

public class UserDto
{
    public string Guid { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Jmbag { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string? ProfilePictureUrl { get; set; } = "/images/default.png";

    public bool Admin { get; set; } = false;

    public DateTime? CreatedAt { get; set; }
}
