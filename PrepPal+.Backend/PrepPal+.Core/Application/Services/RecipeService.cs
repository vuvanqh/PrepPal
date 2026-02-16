using PrepPal_.Core.Application.DTO;
using PrepPal_.Core.Application.DTO.Account;
using PrepPal_.Core.Application.ServiceContracts;
using PrepPal_.Core.ClientContracts;
using PrepPal_.Core.Domain.Entities.RecipeEntities;
using PrepPal_.Core.Domain.RepositoryContracts;
using PrepPal_.Core.Application.Errors;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Core.Application.Services;

public class RecipeService : IRecipeService
{
    private readonly IMealDbClient _mealDbClient;
    private readonly IRecipeRepository _recipeRepository;
    private readonly IRecipeCategoryRepository _recipeCategoryRepository;

    public RecipeService(IMealDbClient mealDbClient, IRecipeRepository recipeRepository, IRecipeCategoryRepository recipeCategoryRepository)
    {
        _mealDbClient = mealDbClient;
        _recipeRepository = recipeRepository;
        _recipeCategoryRepository = recipeCategoryRepository;
        
    }

    public async Task<List<RecipeResponse>> Get10RandomRecipes()
    {
        return await _mealDbClient.Get10RandomRecipes();
    }

    public async Task Interact(UserRecipeInteractionRequest interaction, Guid userId)
    {
        if(!(await _recipeRepository.RecipeExists(interaction.ExternalRecipeId)))
        {
            RecipeResponse? resp = await _mealDbClient.GetRecipeById(interaction.ExternalRecipeId);
            if (resp == null) 
                throw new ExternalServiceException($"Recipe {interaction.ExternalRecipeId} does not exist");
            else
            {
                var categoryId = await _recipeCategoryRepository.GetCategoryIdByName(resp.Category);
                if (categoryId == Guid.Empty)
                    throw new Exception("category id null");
                await _recipeRepository.AddRecipeAsync(new Recipe()
                {
                    Id = Guid.NewGuid(),
                    ExternalId = resp.ExternalId,
                    RecipeName = resp.Name,
                    Area = resp.Area,
                    CategoryId = categoryId,
                    Instructions = resp.Instructions,
                    ImageUrl = resp.ImageUrl
                });
            }
        }

        Recipe? r = (await _recipeRepository.GetRecipeAsync(interaction.ExternalRecipeId));

        if (r == null)
            throw new InvalidOperationException("recipe does not exist");

        await _recipeRepository.AddInteractionAsync(new UserRecipeInteraction()
        {
            Type = interaction.Type,
            UserId = userId,
            RecipeId = r.Id,
            ExternalRecipeId = interaction.ExternalRecipeId,
            TimeStamp = DateTime.UtcNow
        });
    }
}
