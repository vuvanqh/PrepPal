using PrepPal_.Core.Application.DTO.Account;
using PrepPal_.Core.Application.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Core.ServiceContracts;

public interface IRecipeService
{
    Task<List<RecipeResponse>> Get10RandomRecipes();
    Task<List<RecipeResponse>?> SearchRecipesByName(string name);
    Task<Guid> EnsureRecipeExistsAsync(int externalId);
}
