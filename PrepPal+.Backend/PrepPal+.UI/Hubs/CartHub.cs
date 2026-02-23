using Microsoft.AspNetCore.SignalR;

namespace PrepPal_.Backend.Hubs;

public interface ICartHub
{
    Task UpdateCart(Guid cartId);
}

public class CartHub: Hub<ICartHub>
{
}
