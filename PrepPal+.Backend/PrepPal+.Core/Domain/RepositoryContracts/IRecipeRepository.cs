using PrepPal_.Core.Domain.Entities.RecipeEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Core.Domain.RepositoryContracts;

public interface IRecipeRepository
{
    Task AddInteractionAsync(UserRecipeInteraction interaction);
    Task<bool> RecipeExists(int externalId);
    Task AddRecipeAsync(Recipe recipe);
    Task<Recipe?> GetRecipeAsync(int externalId);
}
