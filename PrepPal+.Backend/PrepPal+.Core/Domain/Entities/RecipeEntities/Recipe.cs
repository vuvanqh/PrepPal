using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PrepPal_.Core.Domain.Entities.RecipeEntities;

/// <summary>
/// Recipies that users have interacted with - viewed, liked, shared, added to cart ...
/// </summary>
public class Recipe
{
    [Key]
    public required Guid Id { get; set; }
    public required int ExternalId { get; set; }
    public required string RecipeName { get; set; }
    public required string Area { get; set; }
    public required string Instructions { get; set; }
    public required string ImageUrl { get; set; }


    //rel
    public required Guid CategoryId { get; set; }
    public RecipeCategory? Category { get; set; }
}
