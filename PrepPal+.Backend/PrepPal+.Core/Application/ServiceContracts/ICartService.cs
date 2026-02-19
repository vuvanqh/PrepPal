using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Core;

/// <summary>
/// TO-DO:
/// -add user to cart
/// -add roles
/// -edit cart?
/// </summary>

public interface ICartService
{
    Task AddToCart(Guid userId, Guid cartId, int externalId);
    Task RemoveFromCart(Guid userId, Guid cartId, int externalId);
    Task<CartIdListResponse> GetOwnedCartsAsync(Guid userId);
    Task<CartIdListResponse> GetAccessibleCartsAsync(Guid userId);
    Task<CartResponse?> GetCartContent(Guid userId, Guid cartId);
    Task<CartResponse?> GetCartAsync(Guid userId, Guid cartId);

    Task DeleteCart(Guid userId, Guid cartId);
    Task CreateCart(Guid userId);

}
