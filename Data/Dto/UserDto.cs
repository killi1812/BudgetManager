namespace Data.Dto;

public class UserDto
{
    public string Guid { get; set; } = null!;
    public string Username { get; set; } = null!;
    public bool Admin { get; set; }
}