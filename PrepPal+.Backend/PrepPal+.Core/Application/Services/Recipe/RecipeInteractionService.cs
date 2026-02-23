using PrepPal_.Core.Application.DTO.Account;
using PrepPal_.Core.ClientContracts;
using PrepPal_.Core.Domain.Entities.RecipeEntities;
using PrepPal_.Core.Domain.RepositoryContracts;
using PrepPal_.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Core;

public class RecipeInteractionService : IRecipeInteractionService
{
    private readonly IRecipeRepository _recipeRepository;
    private readonly IRecipeService _recipeService;
    private readonly IMealDbClient _mealDbClient;
    private readonly IUserRepository _userRepository;
    private readonly InteractionDispatcher _interactionDispatcher;
    public RecipeInteractionService(IRecipeRepository recipeRepository, IMealDbClient mealDbClient, InteractionDispatcher interactionDispatcher, IRecipeService recipeService, IUserRepository userRepository)
    {
        _userRepository = userRepository;
        _recipeRepository = recipeRepository;
        _mealDbClient = mealDbClient;
        _interactionDispatcher = interactionDispatcher;
        _recipeService = recipeService;
    }

    public async Task Interact(Guid userId, UserRecipeInteractionRequest request)
    {
        Guid recipeId = await _recipeService.EnsureRecipeExistsAsync(request.ExternalRecipeId);

        UserRecipeInteraction interaction = new UserRecipeInteraction()
        {
            Type = request.Type,
            UserId = userId,
            ExternalRecipeId = request.ExternalRecipeId,
            RecipeId = recipeId,
            TimeStamp = DateTime.UtcNow
        };

        await _interactionDispatcher.Dispatch(interaction, request.Action);
    }

    public async Task<List<LikedRecipeResponse>?> GetLikedRecipes(Guid userId)
    {
        List<Recipe>? recipes =  await _userRepository.GetLikedRecipes(userId);
        return recipes?.Select(r=>r.ToLikedRecipesResponse()).ToList();
    }
}
