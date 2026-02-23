using System;
using System.Collections.Generic;
using System.Text;
using PrepPal_.Core;
using PrepPal_.Core.Domain.Entities.RecipeEntities;

namespace PrepPal_.Core.Domain.RepositoryContracts;

public interface ICartRepository
{
    Task CreateCart(Guid userId);
    Task DeleteCart(Guid cartId, Guid userId);

    Task AddToCartAsync(Guid cartId, Guid userId, Guid recipeId);
    Task RemoveFromCartAsync(Guid cartId, Guid userId, Guid recipeId);

    Task<Cart?> GetCartByIdAsync(Guid userId, Guid cartId);
    Task<List<Cart>?> GetAccessibleCartsAsync(Guid userId);
    Task<List<Cart>?> GetOwnedCartsAsync(Guid userId);

    Task<List<CartRecipe>?> GetCartRecipes(Guid userId,Guid cartId);
    Task<CartResponse?> GetCartDetailsAsync(Guid userId, Guid cartId);

    Task<bool?> HasPermission(Guid userId, Guid cartId, CartAccessType access);

    Task GiveAccessAsync(Guid userId, Guid cartId, CartAccessType access);
    Task RemoveAccessAsync(Guid userId, Guid cartId, CartAccessType access);
}
