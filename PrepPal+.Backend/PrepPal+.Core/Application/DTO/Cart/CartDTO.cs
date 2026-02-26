using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PrepPal_.Core;

public record CartInvitationRequest
{
    [Required] public Guid CartId { get; set; }
    [Required] public string UserName { get; set; } = null!;
    [Required] public CartAccessType Access { get; set; }
}

public record ModifyInvitationStatusRequest
{
    [Required] public Guid InvitationId { get; set; }
    [Required] public Guid CartId { get; set; }
    [Required] public ActionType Action { get; set; }
}

public record ModifyCartAccessRequest
{
    [Required] public Guid CartId { get; set; }
    [Required] public string UserName { get; set; } = null!;
    [Required] public CartAccessType Access { get; set; }
}
