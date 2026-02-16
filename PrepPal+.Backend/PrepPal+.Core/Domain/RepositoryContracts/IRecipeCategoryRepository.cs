using PrepPal_.Core.Application.DTO.Recipes;
using PrepPal_.Core.Domain.Entities.RecipeEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Core.Domain.RepositoryContracts;

public interface IRecipeCategoryRepository
{
    Task AddRangeAsync(CategoryResponse recipeCategories);
    Task<bool> AnyAsync();
    Task<Guid> GetCategoryIdByName(string categoryName);
}
