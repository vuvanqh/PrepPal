using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PrepPal_.Core.Application.DTO;

public record SendMessageRequest
{
    [Required] public string Message { get; set; } = null!;
}

//public record GetConversationRequest
//{
//    [Required] public string
//}
