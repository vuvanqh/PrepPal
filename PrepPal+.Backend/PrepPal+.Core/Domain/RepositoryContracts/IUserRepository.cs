using PrepPal_.Core.Domain.Entities;
using PrepPal_.Core.Domain.Entities.RecipeEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Core.Domain.RepositoryContracts;

public interface IUserRepository
{
    Task<ApplicationUser> GetUserByRefreshToken(string refreshToken);
    Task<ApplicationUser?> GetUserById(Guid id);
    Task<List<Recipe>?> GetLikedRecipes(Guid id);
}
