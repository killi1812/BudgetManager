using Data.Dto;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Services;

public interface IProfileService
{
    Task UpdateProfileDetails(Guid userGuid, UserDto userDto);
    Task UpdateProfilePicture(Guid userGuid, string profilePictureUrl);
}

public class ProfileService : IProfileService
{
    private readonly BudgetManagerContext _context;

    public ProfileService(BudgetManagerContext context)
    {
        _context = context;
    }

    public async Task UpdateProfileDetails(Guid userGuid, UserDto userDto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Guid == userGuid);
        if (user == null) throw new Exception("User not found");

        user.Username = userDto.Username;
        user.ProfilePicture = userDto.ProfilePictureUrl;
        await _context.SaveChangesAsync();
    }

    public async Task UpdateProfilePicture(Guid userGuid, string profilePictureUrl)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Guid == userGuid);
        if (user == null) throw new Exception("User not found");

        user.ProfilePicture = profilePictureUrl;
        await _context.SaveChangesAsync();
    }
}
