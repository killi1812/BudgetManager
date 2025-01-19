using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Data.Hubs;

public class NotificationsHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        {
            var identity = Context.User?.Identity as ClaimsIdentity;
            if (identity != null)
            {
                identity.AddClaim(new Claim("ConnectionId", Context.ConnectionId));
            }
        }
        Console.WriteLine($"Client {Context.ConnectionId} connected");
        //await Clients.All.SendAsync("ReceiveNotification", $"{Context.ConnectionId} has joined");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
            var identity = Context.User?.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var claim = identity.FindFirst("ConnectionId");
                if (claim != null)
                {
                    identity.RemoveClaim(claim);
                }
            }
        Console.WriteLine($"Client {Context.ConnectionId} disconnected");
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendNotification(string message)
    {
        await Clients.All.SendAsync("ReceiveNotification", message);
    }
}