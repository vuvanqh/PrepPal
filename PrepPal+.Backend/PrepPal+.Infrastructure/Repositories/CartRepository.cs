using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using PrepPal_.Core;
using PrepPal_.Core.Domain.Entities.RecipeEntities;
using PrepPal_.Core.Domain.RepositoryContracts;
using PrepPal_.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Infrastructure.Repositories;

public class CartRepository : ICartRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public CartRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }
    public async Task AddToCartAsync(Guid cartId, Guid userId, Guid recipeId)
    {  
        CartRecipe? cartRecipe = await _applicationDbContext.CartRecipeMappings.FirstOrDefaultAsync(cr => cr.CartId == cartId && cr.RecipeId == recipeId);

        if (cartRecipe==null)
        {
            await _applicationDbContext.CartRecipeMappings.AddAsync(new CartRecipe() { CartId = cartId, RecipeId = recipeId , Quantity=1});
        }
        else
        {
            cartRecipe.Quantity++;
            _applicationDbContext.Update(cartRecipe);
        }

        await _applicationDbContext.SaveChangesAsync();

    }

    public async Task GiveAccessAsync(Guid userId, Guid cartId, CartAccessType access)
    {
        CartAccess a = new CartAccess() { CartId = cartId, UserId = userId, AccessType = access };
        await _applicationDbContext.CartAccesses.AddAsync(a);
        await _applicationDbContext.SaveChangesAsync();
    }
    public async Task RemoveAccessAsync(Guid userId, Guid cartId, CartAccessType access)
    {
        CartAccess a = new CartAccess() { CartId = cartId, UserId = userId, AccessType = access };
        _applicationDbContext.CartAccesses.Remove(a);
        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task CreateCart(Guid userId)
    {
        Cart c = new Cart() { Id = Guid.NewGuid(), OwnerId = userId };
        await _applicationDbContext.Carts.AddAsync(c);
        await GiveAccessAsync(userId, c.Id, CartAccessType.Owner);
        await _applicationDbContext.SaveChangesAsync();

        
    }

    public async Task DeleteCart(Guid cartId, Guid userId)
    {
        Cart c = (await GetCartByIdAsync(userId,cartId))!;
        _applicationDbContext.Carts.Remove(c);
        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task<Cart?> GetCartByIdAsync(Guid userId, Guid cartId)
    {
        return await _applicationDbContext.Carts.FirstOrDefaultAsync(c => c.Id == cartId && c.OwnerId == userId);
    }

    public async Task<List<CartRecipe>?> GetCartRecipes(Guid userId, Guid cartId)
    {
        Cart? c = await GetCartByIdAsync(userId, cartId);
        return c?.Recipes.ToList();
    }

    public async Task<List<Cart>?> GetAccessibleCartsAsync(Guid userId)
    {
        List<CartAccess>? accesses =  await _applicationDbContext.CartAccesses.Where(c => c.UserId == userId).DistinctBy(c=>c.CartId).ToListAsync();
        List<Cart>? carts = new List<Cart>();

        foreach(var c in accesses)
        {
            carts.Add(c.Cart);
        }
        return carts;
    }

    public async Task<List<Cart>?> GetOwnedCartsAsync(Guid userId)
    {
        List<CartAccess>? accesses = await _applicationDbContext.CartAccesses.Where(c => c.UserId == userId && c.AccessType==CartAccessType.Owner)
                                                                            .DistinctBy(c => c.CartId).ToListAsync();
        List<Cart>? carts = new List<Cart>();

        foreach (var c in accesses)
        {
            carts.Add(c.Cart);
        }
        return carts;
    }

    public async Task RemoveFromCartAsync(Guid cartId, Guid userId, Guid recipeId)
    {
        Cart? c = await GetCartByIdAsync(userId, cartId);
        if (c == null) return;

        _applicationDbContext.Carts.Remove(c);
        await _applicationDbContext.SaveChangesAsync();
    }


    public async Task<bool?> HasPermission(Guid userId, Guid cartId, CartAccessType access)
    {
        CartAccessType? a = (await _applicationDbContext.CartAccesses.FirstOrDefaultAsync(ca => ca.CartId == cartId && ca.UserId==userId))?.AccessType;
        return a.HasValue && a.Value <= access;
    }
}
