using PrepPal_.Core.Application.DTO.Account;
using PrepPal_.Core.Application.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Core.Application.ServiceContracts;

public interface IRecipeService
{
    Task<List<RecipeResponse>> Get10RandomRecipes();

    Task Interact(UserRecipeInteractionRequest interaction, Guid userId);
}
