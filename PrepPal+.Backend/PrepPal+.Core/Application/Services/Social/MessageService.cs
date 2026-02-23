using PrepPal_.Core.Domain.Entities;
using PrepPal_.Core.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace PrepPal_.Core;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _messageRepo;
    private readonly IUserRepository _userRepo;
    private readonly IConnectionService _connectionService;
    public MessageService(IMessageRepository messageRepo, IUserRepository userRepo, IConnectionService connectionService)
    {
        _messageRepo = messageRepo;
        _userRepo = userRepo;
        _connectionService = connectionService;
    }


    public async Task<ConversationResponse> GetConversation(Guid myId, Guid connectionId)
    {
        Connection c = await _connectionService.CheckConnectionExistance(myId, connectionId);

        List<Message> messages = await _messageRepo.GetConversationAsync(c.Id);

        return new ConversationResponse()
        {
            ConnectionId = connectionId,
            Messages = messages.Select(m => m.ToMessageResponse()).ToList()
        };
    }


    public async Task<MessageResponse> SendMessage(Guid sender, Guid connectionId, string message)
    {
        Connection c = await _connectionService.CheckConnectionExistance(sender, connectionId);

        Message m = new Message()
        {
            Id = Guid.NewGuid(),
            ConnectionId = c.Id,
            Content = message,
            TimeStamp = DateTime.UtcNow,
            SenderUsername = c.UserId1 == sender ? c.User1.UserName! : c.User2.UserName!
        };
        await _messageRepo.SaveMessageAsync(m);
        return m.ToMessageResponse();
    }
}
