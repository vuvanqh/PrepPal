using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Core.Domain.Entities;

public class GroupMembership
{
    public required Guid GroupId {  get; set; }
    public required Guid UserId { get; set; }
    public required ApplicationGroup? ApplicationGroup { get; set; }
    public required ApplicationUser? ApplicationUser { get; set; }
}
