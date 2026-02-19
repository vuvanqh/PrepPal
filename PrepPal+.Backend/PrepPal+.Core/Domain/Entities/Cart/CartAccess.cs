using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using PrepPal_.Core.Domain.Entities;

namespace PrepPal_.Core;

public class CartAccess
{
    public required Guid CartId { get; set; }
    public required Guid UserId {get;set;}

    public ApplicationUser User { get; set; } = null!;
    public Cart Cart {get;set;} = null!;
    public CartAccessType AccessType { get; set; }
}
