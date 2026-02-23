using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PrepPal_.Core;

public class MessageResponse
{
    [Required] public Guid MessageId { get; set; }
    [Required] public string SenderUsername { get; set; } = string.Empty;
    [Required] public DateTime TimeStamp { get; set; }
    [Required] public string Message { get; set; } = string.Empty;
}

public class ConversationResponse
{
    [Required] public Guid ConnectionId { get; set; }
    [Required] public List<MessageResponse> Messages { get; set; } = new List<MessageResponse>();
}

public static class MessageExtention
{
    public static MessageResponse ToMessageResponse(this Message message)
    { 
        return new MessageResponse()
        {
            MessageId = message.Id,
            SenderUsername = message.SenderUsername,
            TimeStamp = DateTime.SpecifyKind(message.TimeStamp, DateTimeKind.Utc),
            Message = message.Content,
        };
    }
}