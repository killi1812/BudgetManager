namespace Data.Dto;

public class NewUserDto
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Jmbag { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? PhoneNumber { get; set; }
    public string Password { get; set; }
}