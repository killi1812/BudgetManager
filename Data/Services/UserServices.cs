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
    public Task<string> LoginJwt(LoginUserDto user);

    public Task<(ClaimsIdentity claimsIdentity, AuthenticationProperties authProperties)> LoginCookie(string username,
        string password);

    public Task ChangePassword(Guid userGuid, string oldPassword, string newPassword);
    public Task<User> GetUser(Guid parse);
    public Task EditUser(Guid userGuid, User user);
    public Task<List<User>> GetUsers(Guid userGuid);
    Task DeleteUser(Guid adminGuid, Guid guid);
}

public class UserServices : IUserServices
{
    private readonly IMapper _mapper;
    private readonly RwaContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILoggerService _loggerService;

    public UserServices(IMapper mapper, RwaContext context, IServiceProvider serviceProvider,
        ILoggerService loggerService)
    {
        _context = context;
        _mapper = mapper;
        _configuration = serviceProvider.GetRequiredService<IConfiguration>();
        _loggerService = loggerService;
    }

    public async Task CreateUser(NewUserDto userDto)
    {
        var userExist = await _context.Users.FirstOrDefaultAsync(u => u.Username == userDto.Username);
        if (userExist != null)
        {
            _loggerService.Log($"User {userDto.Username} already exists", ThreatLvl.Medium);
            throw new Exception("User already exists");
        }

        var user = new User
        {
            Username = userDto.Username,
            Admin = userDto.Admin,
            Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password)
        };

        _loggerService.Log($"User {user.Username} created");

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task<string> LoginJwt(LoginUserDto userDto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == userDto.Username);
        if (user == null)
            throw new NotFoundException($"User {userDto.Username} not found");

        var result = BCrypt.Net.BCrypt.Verify(userDto.Password, user.Password);
        if (!result)
            throw new UnauthorizedException($"Wrong password for user: {userDto.Username}");

        var tokenHandler = new JwtSecurityTokenHandler();
        byte[] key = Encoding.ASCII.GetBytes(_configuration["key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new("guid", user.Guid.ToString())
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        _loggerService.Log($"User {user.Username} logged in");
        return tokenHandler.WriteToken(token);
    }

    public async Task<(ClaimsIdentity claimsIdentity, AuthenticationProperties authProperties)> LoginCookie(
        string username,
        string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

        if (user == null)
            throw new NotFoundException($"User {username}  not found");

        var result = BCrypt.Net.BCrypt.Verify(password, user.Password);
        if (!result)
            throw new UnauthorizedException($"Wrong password for user: {username}");

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Username),
            new("UserGuid", user.Guid.ToString()),
            new("IsAdmin", user.Admin.ToString())
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

        var result = BCrypt.Net.BCrypt.Verify(oldPassword, user.Password);
        if (!result)
            throw new UnauthorizedException($"Wrong password for user {user.Username}");

        user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
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
        if (user.Username == null)
            throw new Exception("Username cannot be null");
        userOld.Username = user.Username;
        _context.Users.Update(userOld);
        await _context.SaveChangesAsync();
    }

    public async Task<List<User>> GetUsers(Guid userGuid)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Guid == userGuid);
        if (user == null)
            throw new NotFoundException($"User with guid {userGuid} not found");
        if (!user.Admin)
            throw new UnauthorizedException("You need to be admin to see all users");
        return await _context.Users.ToListAsync();
    }

    public Task DeleteUser(Guid adminGuid, Guid guid)
    {
        var admin = _context.Users.FirstOrDefault(u => u.Guid == adminGuid);
        if (admin == null)
            throw new NotFoundException($"User with guid {adminGuid} not found");
        if (!admin.Admin)
            throw new UnauthorizedException("You need to be admin to delete users");
        var user = _context.Users.FirstOrDefault(u => u.Guid == guid);
        if (user == null)
            throw new NotFoundException($"User with guid {guid} not found");
        if (user.Guid == adminGuid)
            throw new Exception("You cannot delete yourself");

        _context.Users.Remove(user);
        return _context.SaveChangesAsync();
    }
}