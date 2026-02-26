using PrepPal_.Core.Domain.Entities.RecipeEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Core.Domain.RepositoryContracts;

public interface IRecipeRepository
{
    Task<bool> RecipeExists(int externalId);
    Task AddRecipeAsync(Recipe recipe);
    Task<Recipe?> GetRecipeAsync(int externalId);
    Task<List<Recipe>> GetAllRecipes();
    Task<Recipe?> GetRecipeById(Guid id);
}
