using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PrepPal_.Core.Domain.Entities.RecipeEntities;

public class RecipeCategory
{
    [Key]
    public required Guid Id { get; set; }
    public required int ExternalId { get; set; }
    public required string CategoryName { get; set; }

    //rel
    public ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
}
