using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Core.Domain.Entities.RecipeEntities;

public enum InteractionType
{
    View,
    Like,
    AddToCart
}

/// <summary>
/// for recommendation system
/// </summary>
public class UserRecipeInteraction
{
    public required InteractionType Type { get; set; }
    public required Guid UserId { get; set; }
    public required Guid RecipeId { get; set; }
    public required DateTime TimeStamp { get; set; }
    
    public ApplicationUser? User { get; set; }
    public Recipe? Recipe { get; set; } 
}
