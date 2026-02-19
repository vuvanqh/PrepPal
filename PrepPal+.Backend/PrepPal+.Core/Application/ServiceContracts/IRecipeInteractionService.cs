using PrepPal_.Core.Application.DTO.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Core.ServiceContracts;

public interface IRecipeInteractionService
{
    Task Interact(Guid userId, UserRecipeInteractionRequest request);
        Task<List<LikedRecipeResponse>?> GetLikedRecipes(Guid userId);
}
