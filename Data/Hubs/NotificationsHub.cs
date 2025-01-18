using Microsoft.AspNetCore.SignalR;

namespace Data.Hubs;

public class NotificationsHub: Hub
{
    public override Task OnConnectedAsync()
    {
        Console.WriteLine($"Client {Context.ConnectionId} connected");
        Clients.All.SendAsync("ReceiveNotification", $"{Context.ConnectionId} has joined");
        return base.OnConnectedAsync();
    }

    public async Task SendNotification(string message)
    {
        await Clients.All.SendAsync(message);
    }
}