using Microsoft.EntityFrameworkCore;
using PrepPal_.Core.Domain.Entities.RecipeEntities;
using PrepPal_.Core.Domain.RepositoryContracts;
using PrepPal_.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Infrastructure.Repositories;
public class RecipeInteractionRepository : IRecipeInteractionRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public RecipeInteractionRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

      public async Task AddInteractionAsync(UserRecipeInteraction interaction)
    {
        UserRecipeInteraction? itr = await GetInteractionAsync(interaction);
        if (itr!=null)
            return;

        await _applicationDbContext.UserRecipeInteractions.AddAsync(interaction);
        await _applicationDbContext.SaveChangesAsync();
    }
    public async Task RemoveInteractionAsync(UserRecipeInteraction interaction)
    {
        UserRecipeInteraction? itr = await GetInteractionAsync(interaction);
        if (itr!=null)
            return;

        _applicationDbContext.UserRecipeInteractions.Remove(interaction);
        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task<UserRecipeInteraction?> GetInteractionAsync(UserRecipeInteraction interaction) => await _applicationDbContext.UserRecipeInteractions.FirstOrDefaultAsync(i => i == interaction);

    public async Task<bool> AnyAsync(UserRecipeInteraction interaction) => await _applicationDbContext.UserRecipeInteractions.AnyAsync(i => i==interaction);
    
}