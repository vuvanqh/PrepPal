using PrepPal_.Core.Domain.Entities.RecipeEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Core;

public class CartRecipe
{
    public required Guid CartId { get; set; }
    public required Guid RecipeId { get; set; }
    public required int Quantity { get; set; } = 0;

    public Cart Cart { get; set; } = null!;
    public Recipe Recipe { get; set; } = null!; 
}
