using PrepPal_.Core.Application.DTO;
using PrepPal_.Core.Application.DTO.Recipes;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Core.ClientContracts;

public interface IMealDbClient
{
    Task<RecipeResponse?> GetRecipeById(int externalId);
    Task<RecipeResponse?> GetRecipeByName(string name);
    Task<RecipeResponse> GetRandomRecipe();

    //multiple
    Task<List<RecipeResponse>?> GetRecipesByFirstLetter(char letter);
    Task<List<RecipeResponse>?> GetRecipesByMainIngredient(string ingredient);
    Task<List<RecipeResponse>?> GetRecipesByArea(string area);
    Task<List<RecipeResponse>> Get10RandomRecipes();

    //Task GetMealsByCategory(); //in question

    Task<CategoryResponse> GetRecipeCategories();
}
