using AutoMapper;
using Data.Models;
using Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Helpers;
using WebApp.ViewModels;

namespace WebApp.Controllers;
[Authorize]
public class NotificationsController : Controller
{
    private readonly INotificationService _notifications;
    private readonly IMapper _mapper;

    public NotificationsController(INotificationService notifications, IMapper mapper)
    {
        _notifications = notifications;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index(bool unread = true)
    {
        var guid = HttpContext.GetUserGuid();
        var not = new List<Notification>();
        if (unread)
            not = await _notifications.GetUnread(guid);
        else
            not = await _notifications.GetAll(guid);
        var vm = _mapper.Map<List<NotificationVM>>(not);
        return View(vm);
    }

    public async Task<IActionResult> GetUnreadNotifications()
    {
        var guid = HttpContext.GetUserGuid();
        var count = await _notifications.CountUnread(guid);
        return Ok(count);
    }

    public async Task<IActionResult> ReadAll()
    {
        var guid = HttpContext.GetUserGuid();
        await _notifications.ReadAll(guid);
        return NoContent();
    }
}
    