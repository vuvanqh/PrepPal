using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PrepPal_.Core.Domain.Entities.RecipeEntities;

public class Ingredient
{
    [Key]
    public required Guid Id { get; set; }
    public required string Name { get; set; }   
    public required string AliasName {get;set;}


    public ICollection<RecipeIngredient> Recipes { get; set; } = new List<RecipeIngredient>();
}
