using PrepPal_.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Core;

public class CartInvitation
{
    public required Guid Id { get; set; }
    public required DateTime Timestamp { get; set; }
    public required CartAccessType AccessType {get; set;}
    public required Status Status { get; set; }

    public required Guid SenderId { get; set; }
    public required Guid CartId { get; set; }
    public required Guid ReceiverId { get; set; }
    public ApplicationUser Receiver { get; set; } = null!;
    public ApplicationUser Sender { get; set; } = null!;
    public Cart Cart { get; set; } = null!;
}
