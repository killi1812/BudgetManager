using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Data.Dto;
using Data.Helpers;
using Data.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Data.Services;

public interface IUserServices
{
    public Task CreateUser(NewUserDto user);

    public Task<(ClaimsIdentity claimsIdentity, AuthenticationProperties authProperties)> LoginCookie(string email,
        string password);

    public Task ChangePassword(Guid userGuid, string oldPassword, string newPassword);
    public Task<User> GetUser(Guid parse);
    public Task EditUser(Guid userGuid, User user);
    public Task<List<User>> GetUsers(Guid userGuid);
    Task UpdateProfileDetails(Guid userGuid, UserDto userDto);
    Task UpdateProfilePicture(Guid userGuid, string profilePictureUrl);
    Task DeleteUser(Guid adminGuid, Guid guid);
}

public class UserServices : IUserServices
{
    private readonly IMapper _mapper;
    private readonly BudgetManagerContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILoggerService _loggerService;

    public UserServices(IMapper mapper, BudgetManagerContext context, IServiceProvider serviceProvider,
        ILoggerService loggerService)
    {
        _context = context;
        _mapper = mapper;
        _configuration = serviceProvider.GetRequiredService<IConfiguration>();
        _loggerService = loggerService;
    }

    public async Task CreateUser(NewUserDto userDto)
    {
        var userExist = await _context.Users
            .FirstOrDefaultAsync(u => u.Jmbag == userDto.Jmbag || u.Email == userDto.Email || u.Username == userDto.Username);
        if (userExist != null)
        {
            _loggerService.Log($"User with JMBAG {userDto.Jmbag}, Email {userDto.Email}, or Username {userDto.Username} already exists");
            throw new Exception("User already exists");
        }

        var user = new User
        {
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            Jmbag = userDto.Jmbag,
            Email = userDto.Email,
            Username = userDto.Username,
            PassHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
            //RoleId = userDto.RoleId ?? _context.Roles.FirstOrDefault(r => r.RoleType == "DefaultRole")?.Idrole,
            ProfilePicture = userDto.ProfilePictureUrl
        };

       // if (user.RoleId == null)
        //    throw new Exception("Default Role is missing in the database.");

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task<(ClaimsIdentity claimsIdentity, AuthenticationProperties authProperties)> LoginCookie(
        string email,
        string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
            throw new NotFoundException($"User {email}  not found");

        var result = BCrypt.Net.BCrypt.Verify(password, user.PassHash);
        if (!result)
            throw new UnauthorizedException($"Wrong password for user: {email}");

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.FirstName),
            new("UserGuid", user.Guid.ToString()),
        };
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
            AllowRefresh = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
            IsPersistent = true,
        };
        return (claimsIdentity, authProperties);
    }

    public async Task ChangePassword(Guid userGuid, string oldPassword, string newPassword)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Guid == userGuid);
        if (user == null)
            throw new NotFoundException($"User with guid {userGuid} not fund");

        var result = BCrypt.Net.BCrypt.Verify(oldPassword, user.PassHash);
        if (!result)
            throw new UnauthorizedException($"Wrong password for user {user.Email}");

        user.PassHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
        await _context.SaveChangesAsync();
    }

    public async Task<User> GetUser(Guid parse)
    {
        var user = await _context.Users
            .Where(u => u.Guid == parse)
            .FirstOrDefaultAsync();
        if (user == null)
            throw new NotFoundException($"User with guid {parse} not found");
        return user;
    }

    public async Task EditUser(Guid userGuid, User user)
    {
        var userOld = await _context.Users.FirstOrDefaultAsync(u => u.Guid == userGuid);
        if (userOld == null)
            throw new NotFoundException($"User with guid {userGuid} not found");
        if (!userGuid.Equals(user.Guid))
            throw new UnauthorizedException("You can only edit your own user");
        //TODO implement throu automapper
        throw new NotImplementedException("User edit");
        if (user.Email == null)
            throw new Exception("Username cannot be null");
        userOld.Email = user.Email;
        _context.Users.Update(userOld);
        await _context.SaveChangesAsync();
    }

    public async Task<List<User>> GetUsers(Guid userGuid)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Guid == userGuid);
        if (user == null)
            throw new NotFoundException($"User with guid {userGuid} not found");
        return await _context.Users.ToListAsync();
    }

    public Task DeleteUser(Guid adminGuid, Guid guid)
    {
        var admin = _context.Users.FirstOrDefault(u => u.Guid == adminGuid);
        if (admin == null)
            throw new NotFoundException($"User with guid {adminGuid} not found");
        var user = _context.Users.FirstOrDefault(u => u.Guid == guid);
        if (user == null)
            throw new NotFoundException($"User with guid {guid} not found");
        if (user.Guid == adminGuid)
            throw new Exception("You cannot delete yourself");

        _context.Users.Remove(user);
        return _context.SaveChangesAsync();
    }
    public async Task UpdateProfileDetails(Guid userGuid, UserDto userDto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Guid == userGuid);
        if (user == null) throw new Exception("User not found");

        user.Username = userDto.Username;
        user.ProfilePicture = userDto.ProfilePictureUrl; // Use ProfilePicture instead of ProfilePictureUrl

        await _context.SaveChangesAsync();
    }

    public async Task UpdateProfilePicture(Guid userGuid, string profilePictureUrl)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Guid == userGuid);
        if (user == null) throw new Exception("User not found");

        user.ProfilePicture = profilePictureUrl; // Use ProfilePicture instead of ProfilePictureUrl

        await _context.SaveChangesAsync();
    }
}