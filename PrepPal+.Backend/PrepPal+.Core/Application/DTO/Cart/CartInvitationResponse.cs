using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace PrepPal_.Core;

public class CartInvitationResponse
{
    [Required] public Guid InvitationId { get; set; }
    [Required] public Guid CartId { get; set; }
    [Required] public string OwnerUserName { get; set; } = null!;
    [Required] public Status Status { get; set; }
}
