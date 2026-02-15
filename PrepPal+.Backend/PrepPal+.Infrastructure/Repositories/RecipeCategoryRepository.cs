using Microsoft.EntityFrameworkCore;
using PrepPal_.Core.Application.DTO.Recipes;
using PrepPal_.Core.Domain.Entities.RecipeEntities;
using PrepPal_.Core.Domain.RepositoryContracts;
using PrepPal_.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Infrastructure.Repositories;

public class RecipeCategoryRepository : IRecipeCategoryRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public RecipeCategoryRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task AddRangeAsync(CategoryResponse recipeCategories)
    {
        await _applicationDbContext.RecipeCategories.AddRangeAsync(recipeCategories.ToRecipeCategories());
    }
    public async Task<bool> AnyAsync() => await _applicationDbContext.RecipeCategories.AnyAsync();
}
