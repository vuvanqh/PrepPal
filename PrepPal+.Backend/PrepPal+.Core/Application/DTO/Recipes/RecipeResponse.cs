using PrepPal_.Core.Domain.Entities.RecipeEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace PrepPal_.Core.Application.DTO;

public class RecipeResponse
{
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [Required] public int ExternalId { get; set; }
    [Required] public string Name { get; set; } = null!;
    [Required] public string Category {  get; set; } = null!;
    [Required] public string Area {  get; set; } = null!;
    [Required] public string Instructions { get; set; } = null!;
    [Required] public string ImageUrl { get; set; } = null!;
 
    [Required] public List<IngredientDTO> Ingredients { get; set; } = new List<IngredientDTO>();

}

public static class RecipeResponseExtention
{
    public static RecipeResponse ToRecipeResponse(this Recipe recipe)
    {
        return new RecipeResponse()
        {
            ExternalId = recipe.ExternalId,
            Name = recipe.RecipeName,
            Area = recipe.Area,
            Instructions = recipe.Instructions,
            ImageUrl = recipe.ImageUrl,
            //ADD INGREDIENTS IMPORTANT
        };
    }
}