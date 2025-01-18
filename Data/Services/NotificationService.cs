using Data.Helpers;
using Data.Hubs;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Services;

public interface INotificationService
{
    public Task<List<Notification>> GetAll(Guid userGuid);
    public Task<List<Notification>> GetUnread(Guid userGuid);
    public Task Create(Guid userGuid, string message);
}

public class NotificationService : INotificationService
{
    private readonly BudgetManagerContext _context;

    public NotificationService(BudgetManagerContext context)
    {
        _context = context;
    }

    public async Task<List<Notification>> GetAll(Guid userGuid)
    {
        return await _context.Notifications
            .AsNoTracking()
            .Where(n => n.User.Guid == userGuid)
            .ToListAsync();
    }

    public async Task<List<Notification>> GetUnread(Guid userGuid)
    {
        return await _context.Notifications
            .AsNoTracking()
            .Where(n => n.User.Guid == userGuid)
            .Where(n => !n.Read)
            .ToListAsync();
    }

    public async Task Create(Guid userGuid, string message)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Guid == userGuid);
        if (user == null) throw new NotFoundException($"User with guid: {userGuid} not found");
        var newNot = new Notification
        {
            UserId = user.Iduser,
            Message = message,
        };
        await _context.Notifications.AddAsync(newNot);
        await _context.SaveChangesAsync();
        NotifyUser(userGuid);
    }

    private void NotifyUser(Guid userGuid)
    {
    }
}