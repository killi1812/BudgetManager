using AutoMapper;
using Data.Dto;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Services;

public interface IUserManagementService
{
    Task CreateUser(NewUserDto userDto);
    Task<User> GetUser(Guid userGuid);
    Task EditUser(Guid userGuid, UserDto userDto);
    Task<List<User>> GetUsers();
    Task DeleteUser(Guid userGuid);
}

public class UserManagementService : IUserManagementService
{
    private readonly BudgetManagerContext _context;
    private readonly IMapper _mapper;

    public UserManagementService(BudgetManagerContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task CreateUser(NewUserDto userDto)
    {
        var userExist = await _context.Users
            .FirstOrDefaultAsync(u => u.Jmbag == userDto.Jmbag || u.Email == userDto.Email || u.Username == userDto.Username);

        if (userExist != null)
            throw new Exception("User already exists");

        var user = _mapper.Map<User>(userDto);
        user.Guid = Guid.NewGuid();
        user.PassHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
        user.CreatedAt = DateTime.UtcNow;

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User> GetUser(Guid userGuid)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Guid == userGuid);
        if (user == null)
            throw new Exception($"User with GUID {userGuid} not found");
        return user;
    }

    public async Task EditUser(Guid userGuid, UserDto userDto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Guid == userGuid);
        if (user == null)
            throw new Exception($"User with GUID {userGuid} not found");

        _mapper.Map(userDto, user);
        await _context.SaveChangesAsync();
    }

    public async Task<List<User>> GetUsers()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task DeleteUser(Guid userGuid)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Guid == userGuid);
        if (user == null)
            throw new Exception($"User with GUID {userGuid} not found");

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }
}
