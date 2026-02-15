using PrepPal_.Core.Application.DTO;
using PrepPal_.Core.Application.DTO.Recipes;
using PrepPal_.Core.Application.Errors;
using PrepPal_.Core.ClientContracts;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Net.Http.Json;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PrepPal_.Infrastructure.Clients;

public class MealDbClient : IMealDbClient
{
    private readonly HttpClient _httpClient;

    public MealDbClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }


    public async Task<List<RecipeResponse>> Get10RandomRecipes()
    {
        List<RecipeResponse> recipes = new List<RecipeResponse>();
        int limit = 30;

        for (int i = 0; i < 10; i++)
        {
            if (limit-- < 0) break;

            RecipeResponse resp = await GetRandomRecipe();
            if (recipes.Any(r => r.Name == resp.Name))
            {
                i--;
                continue;
            }
            recipes.Add(resp);
        }
        return recipes;
    }

    //public Task GetMealsByCategory()
    //{
    //    throw new NotImplementedException();
    //}

    public async Task<RecipeResponse> GetRandomRecipe()
    {
        var data = await FetchHelper("random.php");
        
        RecipeResponse recipe = data!.Meals!.Select(meal => meal.ToRecipeResponse()).FirstOrDefault()!;

        return recipe;

    }

    public async Task<List<RecipeResponse>?> GetRecipesByFirstLetter(char letter)
    {
        List<RecipeResponse> recipes = new List<RecipeResponse>();

        var data = await FetchHelper($"search.php?f={letter}");

        return data?.Meals?.Select(m=>m.ToRecipeResponse()).ToList();

    }

    public async Task<RecipeResponse?> GetRecipeById(int id)
    {
        var data = await FetchHelper($"lookup.php?i={id}");

        RecipeResponse? recipe = data?.Meals?.Select(m => m.ToRecipeResponse()).FirstOrDefault();
        return recipe;
    }

    public async Task<RecipeResponse?> GetRecipeByName(string name)
    {
        var data = await FetchHelper($"search.php?s={name}");

        RecipeResponse? recipe = data?.Meals?.Select(m => m.ToRecipeResponse()).FirstOrDefault();
        return recipe;
    }

    public async Task<List<RecipeResponse>?> GetRecipesByArea(string area)
    {
        var data = await FetchHelper($"filter.php?a={area}");

        return data?.Meals?.Select(m => m.ToRecipeResponse()).ToList();
    }

    public async Task<List<RecipeResponse>?> GetRecipesByMainIngredient(string ingredient)
    {
        var data = await FetchHelper($"filter.php?i={ingredient}");

        return data?.Meals?.Select(m => m.ToRecipeResponse()).ToList();
    }

    public async Task<CategoryResponse> GetRecipeCategories()
    {
        var response = await _httpClient.GetAsync("categories.php");

        if (!response.IsSuccessStatusCode)
            throw new ExternalServiceException($"TheMealDB failed to fetch categories with status code {response.StatusCode}");

        return (await response.Content.ReadFromJsonAsync<CategoryResponse>())!;
    }



    //private helpers
    private async Task<MealDbResponse?> FetchHelper(string searchQuery)
    {
        var response = await _httpClient.GetAsync(searchQuery);
        if (!response.IsSuccessStatusCode)
            throw new ExternalServiceException($"TheMealDB failed to fetch meal(s) with status code {response.StatusCode}");

        return await response.Content.ReadFromJsonAsync<MealDbResponse>();
    }
}
