using Microsoft.AspNetCore.SignalR;

namespace Sistema_Alertas.Services.NewFolder;


public class NotificationHub : Hub
{
    public async Task SendNotification(string message)
    {
        await Clients.All.SendAsync("ReceiveNotification", message);
    }
}
