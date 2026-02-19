

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using PrepPal_.Core.Application.DTO;
using PrepPal_.Core.Domain.Entities.RecipeEntities;

public class LikedRecipeResponse
{
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [Required] public int ExternalId { get; set; }
    [Required] public string Name { get; set; } = null!;
    [Required] public string Category {  get; set; } = null!;
    [Required] public string Area {  get; set; } = null!;
    [Required] public string Instructions { get; set; } = null!;
    [Required] public string ImageUrl { get; set; } = null!;
}

public static class RecipeExtention
{
    public static LikedRecipeResponse ToLikedRecipesResponse(this Recipe recipe)
    {
        return new LikedRecipeResponse()
        {
            ExternalId = recipe.ExternalId,
            Name = recipe.RecipeName,
            Category = recipe.Category!.CategoryName,
            Area = recipe.Area,
            Instructions = recipe.Instructions,
            ImageUrl = recipe.ImageUrl
        };
    }
}