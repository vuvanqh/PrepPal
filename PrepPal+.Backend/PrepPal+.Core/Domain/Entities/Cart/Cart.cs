using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using PrepPal_.Core.Domain.Entities;

namespace PrepPal_.Core;

public class Cart
{
    public required Guid Id { get; set; }
    public required Guid OwnerId {get;set;}

    public ApplicationUser Owner {get;set;} = null!;
    public ICollection<CartAccess> Accesses { get; set; } = new List<CartAccess>();
    public ICollection<CartRecipe> Recipes { get; set; } = new List<CartRecipe>();
}
