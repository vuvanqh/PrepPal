using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace PrepPal_.Core.Domain.Entities.RecipeEntities;


/// <summary>
/// for recommendation system
/// </summary>
public class UserRecipeInteraction: IEquatable<UserRecipeInteraction>
{
    public required InteractionType Type { get; set; }
    public required Guid UserId { get; set; }
    public required Guid RecipeId { get; set; }
    public required int ExternalRecipeId { get; set; }
    public required DateTime TimeStamp { get; set; }
    
    public ApplicationUser User { get; set; } = null!;
    public Recipe Recipe { get; set; } = null!;

    public bool Equals(UserRecipeInteraction? other)
    {
        return other?.UserId == UserId && other?.RecipeId == RecipeId && other?.Type == Type;
    }
}
