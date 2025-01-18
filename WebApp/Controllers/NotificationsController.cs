using AutoMapper;
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

    public async Task<IActionResult> Index()
    {
        var guid = HttpContext.GetUserGuid();
        var not = await _notifications.GetUnread(guid);
        var vm = _mapper.Map<List<NotificationVM>>(not);
        return View(vm);
    }
}
    