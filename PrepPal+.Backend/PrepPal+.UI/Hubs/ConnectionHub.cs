using Microsoft.AspNetCore.SignalR;
using PrepPal_.Core;
using PrepPal_.Core.DTO;

namespace PrepPal_.Backend.Hubs;

public interface INotificationClient
{
    Task ReceiveConnectionRequestNotification(string username);
    Task NotifyConnectionAccepted(string username);
}

public class NotificationHub : Hub<INotificationClient>
{
}
