using PrepPal_.Core.Application.DTO;
using PrepPal_.Core.ServiceContracts;
using PrepPal_.Core.ClientContracts;
using PrepPal_.Core.Domain.Entities.RecipeEntities;
using PrepPal_.Core.Domain.RepositoryContracts;
using PrepPal_.Core.Application.Errors;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PrepPal_.Core.Application.Services;

public class RecipeService : IRecipeService
{
    private readonly IMealDbClient _mealDbClient;
    private readonly IRecipeRepository _recipeRepository;
    private readonly IRecipeCategoryRepository _recipeCategoryRepository;
    private readonly IIngredientRepository _ingredientRepository;

    public RecipeService(IMealDbClient mealDbClient, IRecipeRepository recipeRepository, IRecipeCategoryRepository recipeCategoryRepository, IIngredientRepository ingredientRepository)
    {
        _mealDbClient = mealDbClient;
        _recipeRepository = recipeRepository;
        _recipeCategoryRepository = recipeCategoryRepository;
        _ingredientRepository = ingredientRepository;
        
    }

    public async Task<List<RecipeResponse>> Get10RandomRecipes()
    {
        return await _mealDbClient.Get10RandomRecipes();
    }

    public async Task<List<RecipeResponse>?> SearchRecipesByName(string name)
    {
        List<RecipeResponse> resp =  await _mealDbClient.GetRecipeByName(name.ToLower())?? new List<RecipeResponse>();

        return resp;
    }

    public async Task<Guid> EnsureRecipeExistsAsync(int externalId)
    {
        Recipe r;
        List<IngredientDTO>? ingredients = null;

        if(await _recipeRepository.RecipeExists(externalId))
            r = (await _recipeRepository.GetRecipeAsync(externalId))!;
        else {
            RecipeResponse resp = await _mealDbClient.GetRecipeById(externalId)??throw new InvalidOperationException("Recipe does not exist");
            ingredients = resp.Ingredients;
            r = new Recipe()
            {
                Id = Guid.NewGuid(),
                ExternalId = externalId,
                RecipeName = resp.Name,
                Area = resp.Area,
                CategoryId = (await _recipeCategoryRepository.GetCategoryIdByName(resp.Category)),
                ImageUrl = resp.ImageUrl,
                Instructions = resp.Instructions
            };
            await _recipeRepository.AddRecipeAsync(r);
        }
        if(ingredients!=null)
        {
            foreach(var i in ingredients)
            {
                string normalizedName = NormalizeBasic(i.IngredientName); 
                await _ingredientRepository.AddIngredientAsync(normalizedName, i.IngredientName);
                Guid ingredientId = (await _ingredientRepository.GetIngredientByName(normalizedName))!.Id;
                await _ingredientRepository.AddIngredientRepoMapping(r.Id, ingredientId , i.IngredientMeasure);
            }
        }

        return r.Id;
    }
        
    public async Task<RecipeResponse> GetRecipeById(Guid id)
    {
        Recipe? r = await _recipeRepository.GetRecipeById(id);
        if (r == null) throw new ArgumentException("recipe does not exist");

        return r.ToRecipeResponse();
    }

    public async Task<List<RecipeResponse>> FillResponseList(List<RecipeResponse> list)
    {
        List<Recipe> r  = await _recipeRepository.GetAllRecipes();
        List<RecipeResponse> resp = r.Where(r => !list.Contains(r.ToRecipeResponse())).Select(r=>r.ToRecipeResponse()).ToList();
        list.AddRange(resp);
        return list;
    }

    private string NormalizeBasic(string input)
    {
        input = input.ToLowerInvariant();

        input = Regex.Replace(input, @"\b\d+\b", ""); 
        input = Regex.Replace(input, @"\b(large|small|fresh|chopped|minced|dried)\b", "");
        input = Regex.Replace(input, @"[^a-z\s]", "");
        input = Regex.Replace(input, @"\s+", " ").Trim();


        if (input.EndsWith("s") && input.Length > 3)
            input = input[..^1];

        return input;
    }
}
