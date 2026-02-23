using Microsoft.AspNetCore.SignalR;
using PrepPal_.Core;

namespace PrepPal_.Backend.Hubs;


public interface IChatClient
{
    Task ReceiveMessage(MessageResponse message, Guid connectionId);
}

public class ChatHub: Hub<IChatClient>
{
    public override Task OnConnectedAsync()
    {
        Console.WriteLine(
            $"ChatHub connected: {Context.UserIdentifier}"
        );
        return base.OnConnectedAsync();
    }
}
