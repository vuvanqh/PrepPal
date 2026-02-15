using Microsoft.EntityFrameworkCore.Query.Internal;
using PrepPal_.Core.Application.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Text;

namespace PrepPal_.Infrastructure.Clients;

public class MealDbDTO
{
    [Required] public int idMeal { get; set;  }
    [Required] public string strMeal { get; set; } = null!;
    [Required] public string strCategory { get; set; } = null!;
    [Required] public string strArea { get; set; } = null!;
    [Required] public string strInstructions { get; set; } = null!;
    [Required] public string strMealThumb { get; set; } = null!;
    #region ingredients
    public string? strIngredient1 { get; set; }
    public string? strIngredient2 { get; set; }
    public string? strIngredient3 { get; set; }
    public string? strIngredient4 { get; set; }
    public string? strIngredient5 { get; set; }
    public string? strIngredient6 { get; set; }
    public string? strIngredient7 { get; set; }
    public string? strIngredient8 { get; set; }
    public string? strIngredient9 { get; set; }
    public string? strIngredient10 { get; set; }
    public string? strIngredient11{ get; set; }
    public string? strIngredient12 { get; set; }
    public string? strIngredient13 { get; set; }
    public string? strIngredient14 { get; set; }
    public string? strIngredient15 { get; set; }
    public string? strIngredient16 { get; set; }
    public string? strIngredient17 { get; set; }
    public string? strIngredient18 { get; set; }
    public string? strIngredient19 { get; set; }
    public string? strIngredient20 { get; set; }
    #endregion
    #region measurements
    public string? strMeasure1 { get; set; }
    public string? strMeasure2 { get; set; }
    public string? strMeasure3 { get; set; }
    public string? strMeasure4 { get; set; }
    public string? strMeasure5 { get; set; }
    public string? strMeasure6 { get; set; }
    public string? strMeasure7 { get; set; }
    public string? strMeasure8 { get; set; }
    public string? strMeasure9 { get; set; }
    public string? strMeasure10 { get; set; }
    public string? strMeasure11 { get; set; }
    public string? strMeasure12 { get; set; }
    public string? strMeasure13 { get; set; }
    public string? strMeasure14 { get; set; }
    public string? strMeasure15 { get; set; }
    public string? strMeasure16 { get; set; }
    public string? strMeasure17 { get; set; }
    public string? strMeasure18 { get; set; }
    public string? strMeasure19 { get; set; }
    public string? strMeasure20 { get; set; }
    #endregion


    public RecipeResponse ToRecipeResponse()
    {
        return new RecipeResponse()
        {
            ExternalId = idMeal,
            Name = strMeal,
            Category = strCategory,
            Area = strArea,
            Instructions = strInstructions,
            ImageUrl = strMealThumb,
            Ingredients = GetIngredients()
        };
    }

    private List<IngredientDTO> GetIngredients()
    {
        List<IngredientDTO> ingredients = new List<IngredientDTO>();
        for(int i = 1; i <= 20; i++)
        {
            var ingredient = this.GetType().GetProperty($"strIngredient{i}")?.GetValue(this) as string;
            var measure = this.GetType().GetProperty($"strMeasure{i}")?.GetValue(this) as string;

            if (String.IsNullOrEmpty(ingredient))
                break;

            ingredients.Add(new IngredientDTO() { 
                IngredientName = ingredient,
                IngredientMeasure = measure??""
            });
        }
        return ingredients;
    }
}


public class MealDbResponse
{
    [Required] public List<MealDbDTO>? Meals { get; set; }
}
