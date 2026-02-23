using PrepPal_.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Core;

public class Connection
{
    public required Guid Id { get; set; }
    public required Guid UserId1 { get; set; }
    public required Guid UserId2 { get; set; }
    public required Guid RequestedByUserId { get; set; }
    public required Status Status { get; set; }


    public ApplicationUser User1 { get; set; } = null!;
    public ApplicationUser User2 { get; set; } = null!;


    public ICollection<Message> Messages { get; set; } = new List<Message>();
}

