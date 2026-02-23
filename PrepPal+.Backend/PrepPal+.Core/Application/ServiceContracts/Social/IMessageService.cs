using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Core;

public interface IMessageService
{
    Task<MessageResponse> SendMessage(Guid sender, Guid connectionId, string message);
    Task<ConversationResponse> GetConversation(Guid myId, Guid connectionId);
}
