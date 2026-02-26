using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PrepPal_.Core.Application.DTO.Social;

public record MakeConnectionRequest
{
    [Required] public string UserName { get; set; } = null!;
}

public record ConnectionActionRequest
{
    [Required] public Guid ConnectionId { get; set; }
    [Required] public ActionType Action { get; set; }
}