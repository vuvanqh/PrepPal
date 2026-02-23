using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Core;

public interface IMessageRepository
{
    Task<List<Message>> GetConversationAsync(Guid conversationId);
    Task SaveMessageAsync(Message message);
}
