using Data.Helpers;
using Data.Hubs;
using Data.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Data.Services;

public interface INotificationService
{
    public Task<List<Notification>> GetAll(Guid userGuid);
    public Task<List<Notification>> GetUnread(Guid userGuid);
    public Task Create(Guid userGuid, string message);
    Task<int> CountUnread(Guid guid);
    Task ReadAll(Guid guid);
}

public class NotificationService : INotificationService
{
    private readonly BudgetManagerContext _context;
    private readonly IHubContext<NotificationsHub> _hubContext;

    public NotificationService(BudgetManagerContext context, IHubContext<NotificationsHub> hubContext)
    {
        _context = context;
        _hubContext = hubContext;
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
        await NotifyUser(userGuid);
    }

    public async Task<int> CountUnread(Guid guid)
    {
        var count = await  _context.Notifications.AsNoTracking()
            .Where(n => n.User.Guid == guid)
            .CountAsync(n => !n.Read);
        return count;
    }

    public async Task ReadAll(Guid guid)
    {
        var notifications = await _context.Notifications
            .Where(n => n.User.Guid == guid)
            .ToListAsync();
        foreach (var n in notifications)
        {
            n.Read = true;
        }
        await _context.SaveChangesAsync();
    }

    private async Task NotifyUser(Guid userGuid)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveNotification", userGuid.ToString());
    }
}