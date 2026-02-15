using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Core.Domain.Entities.RecipeEntities;

public class RecipeIngredient
{
    public required Guid RecipeId { get; set; }
    public required Guid IngredientId { get; set; }
    public required string Measurement { get; set; }

    public Recipe? Recipe { get; set; }
    public Ingredient? Ingredient { get; set; }
}
