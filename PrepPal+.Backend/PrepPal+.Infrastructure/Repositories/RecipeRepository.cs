using Microsoft.EntityFrameworkCore;
using PrepPal_.Core.Domain.Entities.RecipeEntities;
using PrepPal_.Core.Domain.RepositoryContracts;
using PrepPal_.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Infrastructure.Repositories;

public class RecipeRepository : IRecipeRepository
{
    private readonly ApplicationDbContext _applDbContext;
    public RecipeRepository(ApplicationDbContext applDbContext)
    {
        _applDbContext = applDbContext;
    }

    public async Task AddInteractionAsync(UserRecipeInteraction interaction)
    {
        if (await _applDbContext.UserRecipeInteractions.AnyAsync(i => i.UserId == interaction.UserId && i.ExternalRecipeId == interaction.ExternalRecipeId && i.Type == interaction.Type))
            return;

        await _applDbContext.UserRecipeInteractions.AddAsync(interaction);
        await _applDbContext.SaveChangesAsync();
    }

    public async Task<bool> RecipeExists(int externalId)
    {
        return await _applDbContext.Recipes.AnyAsync(r => r.ExternalId== externalId);
    }

    public async Task AddRecipeAsync(Recipe recipe)
    {
        await _applDbContext.Recipes.AddAsync(recipe);
        await _applDbContext.SaveChangesAsync();
    }

    public async Task<Recipe?> GetRecipeAsync(int externalId) => await _applDbContext.Recipes.FirstOrDefaultAsync(r => r.ExternalId == externalId);


}
