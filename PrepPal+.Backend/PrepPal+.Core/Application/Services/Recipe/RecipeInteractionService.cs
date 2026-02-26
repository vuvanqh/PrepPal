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
    private readonly IRecipeInteractionRepository _interactionRepository;
    public RecipeInteractionService(IRecipeRepository recipeRepository, IMealDbClient mealDbClient, InteractionDispatcher interactionDispatcher, IRecipeService recipeService, IUserRepository userRepository, IRecipeInteractionRepository interactionRepository)
    {
        _userRepository = userRepository;
        _recipeRepository = recipeRepository;
        _mealDbClient = mealDbClient;
        _interactionDispatcher = interactionDispatcher;
        _recipeService = recipeService;
        _interactionRepository = interactionRepository;
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
    public async Task<RecommendationRequest> GetRecommendationRequestData(Guid userId, InteractionType type)
    {
        List<Recipe> recipes = await _recipeRepository.GetAllRecipes();
        List<UserRecipeInteraction> interactions = (await _interactionRepository.GetAllInteractions()).Where(i => i.Type == type && i.UserId==userId).ToList();

        return new RecommendationRequest()
        {
            recipes = recipes.Select(r => new RecommendationRecipeReq()
            {
                recipeId = r.Id,
                category = r.Category.CategoryName,
                area = r.Area,
                ingredients = r.RecipeIngredients.Select(i => i.IngredientId.ToString()).ToList(),
            }).ToList(),
            likes = interactions.Select(i => i.RecipeId).ToList()
        };
    }
}
