using PrepPal_.Core.Domain.Entities.RecipeEntities;
using System;

namespace PrepPal_.Core.Domain.RepositoryContracts;
public interface IRecipeInteractionRepository
{
    Task AddInteractionAsync(UserRecipeInteraction interaction);
    Task RemoveInteractionAsync(UserRecipeInteraction interaction);
    Task<bool> AnyAsync(UserRecipeInteraction interaction);
    Task<UserRecipeInteraction?> GetInteractionAsync(UserRecipeInteraction interaction);
    Task<List<UserRecipeInteraction>> GetAllInteractions();
}