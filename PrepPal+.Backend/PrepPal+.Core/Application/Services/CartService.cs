using PrepPal_.Core.Application.DTO;
using PrepPal_.Core.Application.Services;
using PrepPal_.Core.Domain.Entities.RecipeEntities;
using PrepPal_.Core.Domain.RepositoryContracts;
using PrepPal_.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PrepPal_.Core;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepo;
    private readonly IRecipeRepository _recipeRepo;
    private readonly IRecipeService _recipeService;
    
    public CartService(ICartRepository cartRepo, IRecipeRepository recipeRepo, IRecipeService recipeService)
    {
        _cartRepo = cartRepo;
        _recipeRepo = recipeRepo;
        _recipeService = recipeService;
    }

    public async Task AddToCart(Guid userId, Guid cartId, int externalId)
    {
        bool? canEdit = await _cartRepo.HasPermission(userId, cartId, CartAccessType.Editor);
        if (!canEdit.HasValue || !canEdit.Value)
            throw new UnauthorizedAccessException("No permission");

        Guid recipeId = await _recipeService.EnsureRecipeExistsAsync(externalId);

        await _cartRepo.AddToCartAsync(userId, cartId, recipeId);
    }


    public async Task RemoveFromCart(Guid userId, Guid cartId, int externalId)
    {
        bool? canEdit = await _cartRepo.HasPermission(userId, cartId, CartAccessType.Editor);
        if (!canEdit.HasValue || !canEdit.Value)
            throw new UnauthorizedAccessException("No permission");

        Recipe? r = await _recipeRepo.GetRecipeAsync(externalId);
        if (r == null) return;

        await _cartRepo.RemoveFromCartAsync(userId, cartId, r.Id);
    }


    public async Task CreateCart(Guid userId)
    {
        if ((await _cartRepo.GetOwnedCartsAsync(userId))?.Count() > 10)
            throw new InvalidOperationException("You own too many carts. Delete some before creating a new one");
        await _cartRepo.CreateCart(userId);
    }

    public async Task DeleteCart(Guid userId, Guid cartId)
    {
        bool? owns = await _cartRepo.HasPermission(userId, cartId, CartAccessType.Owner);
        if (!owns.HasValue || !owns.Value)
            throw new UnauthorizedAccessException("No permission");

        await _cartRepo.DeleteCart(userId, cartId);
    }

    public async Task<CartIdListResponse> GetAccessibleCartsAsync(Guid userId)
    {
        List<Cart>? carts = await _cartRepo.GetAccessibleCartsAsync(userId);
        return new CartIdListResponse() { CartIdList = carts?.Select(c => c.Id)?.ToList()?? new List<Guid>() };
    }

    public async Task<CartIdListResponse> GetOwnedCartsAsync(Guid userId)
    {
       List<Cart>? carts = await _cartRepo.GetOwnedCartsAsync(userId);
       return new CartIdListResponse() { CartIdList = carts?.Select(c => c.Id)?.ToList() ?? new List<Guid>() };
    }

    public async Task<CartResponse?> GetCartAsync(Guid userId, Guid cartId)
    {

        bool? owns = await _cartRepo.HasPermission(userId, cartId, CartAccessType.Viewer);
        if (!owns.HasValue || !owns.Value)
            throw new UnauthorizedAccessException("No permission");

        Cart? c = await _cartRepo.GetCartByIdAsync(userId, cartId);
        return c?.ToCartResponse();
    }

    public async Task<CartResponse?> GetCartContent(Guid userId, Guid cartId)
    {
        Cart? c = await _cartRepo.GetCartByIdAsync(userId, cartId);
       
        return c.ToCartResponse();
    }
}
