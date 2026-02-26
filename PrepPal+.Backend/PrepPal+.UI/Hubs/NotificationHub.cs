using Microsoft.AspNetCore.SignalR;
using PrepPal_.Core;
using PrepPal_.Core.Domain.RepositoryContracts;
using PrepPal_.Core.DTO;
using System.Security.Claims;

namespace PrepPal_.Backend.Hubs;

public interface INotificationClient
{
    Task ReceiveConnectionRequestNotification(string username);
    Task NotifyConnectionAccepted(string username);


    Task ReceiveCartInvitationNotification(string username);
    Task NotifyCartInvitationAccepted(string username);

    Task RemoveFromCart(Guid cartId);
    Task UpdateCart(Guid cartId);
}

public class NotificationHub : Hub<INotificationClient>
{
    private readonly ICartRepository _cartRepo;
    public NotificationHub(ICartRepository cartRepository)
    {
        _cartRepo = cartRepository;
    }

    public async Task JoinCart(Guid cartId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, cartId.ToString());
    }

    public async Task LeaveCart(Guid cartId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, cartId.ToString());
    }
}
