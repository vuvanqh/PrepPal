using PrepPal_.Core.Application.DTO;
using PrepPal_.Core.DTO.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Core.ServiceContracts;

public interface IAccountService
{
    Task<PersonalDetailsResponse> GetPersonalDetails(Guid userId);
    Task<List<RecipeResponse>?> GetLikedRecipes(Guid userId);
}
