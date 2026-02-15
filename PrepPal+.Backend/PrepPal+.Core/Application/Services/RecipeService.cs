using PrepPal_.Core.Application.DTO;
using PrepPal_.Core.Application.ServiceContracts;
using PrepPal_.Core.ClientContracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Core.Application.Services;

public class RecipeService : IRecipeService
{
    private readonly IMealDbClient _mealDbClient;

    public RecipeService(IMealDbClient mealDbClient)
    {
        _mealDbClient = mealDbClient;
    }

    public async Task<List<RecipeResponse>> Get10RandomRecipes()
    {
        return await _mealDbClient.Get10RandomRecipes();
    }
}
