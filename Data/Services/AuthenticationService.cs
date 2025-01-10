using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Services;

public interface IAuthenticationService
{
    Task<(ClaimsIdentity claimsIdentity, AuthenticationProperties authProperties)> Login(string email, string password);
    Task ChangePassword(Guid userGuid, string oldPassword, string newPassword);
}

public class AuthenticationService : IAuthenticationService
{
    private readonly BudgetManagerContext _context;

    public AuthenticationService(BudgetManagerContext context)
    {
        _context = context;
    }

    public async Task<(ClaimsIdentity claimsIdentity, AuthenticationProperties authProperties)> Login(string email, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null) throw new Exception($"User {email} not found");

        if (!BCrypt.Net.BCrypt.Verify(password, user.PassHash)) throw new Exception("Invalid password");

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.FirstName),
            new("UserGuid", user.Guid.ToString())
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
            AllowRefresh = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
            IsPersistent = true
        };

        return (claimsIdentity, authProperties);
    }

    public async Task ChangePassword(Guid userGuid, string oldPassword, string newPassword)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Guid == userGuid);
        if (user == null) throw new Exception($"User with GUID {userGuid} not found");

        if (!BCrypt.Net.BCrypt.Verify(oldPassword, user.PassHash)) throw new Exception("Old password is incorrect");

        user.PassHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
        await _context.SaveChangesAsync();
    }
}
