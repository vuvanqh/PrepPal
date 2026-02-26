using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using PrepPal_.Core;
using PrepPal_.Core.Application.DTO;
using PrepPal_.Core.Application.DTO.Recipes;
using PrepPal_.Core.Domain.Entities.RecipeEntities;
using PrepPal_.Core.Domain.RepositoryContracts;
using PrepPal_.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace PrepPal_.Infrastructure.Repositories;

public class CartRepository : ICartRepository
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly ILogger<CartRepository> _logger;

    public CartRepository(ApplicationDbContext applicationDbContext, ILogger<CartRepository> logger)
    {
        _applicationDbContext = applicationDbContext;
        _logger = logger;
    }
    public async Task AddToCartAsync(Guid cartId, Guid userId, Guid recipeId)
    {
        Console.WriteLine("CartId being inserted: " + cartId);
        Console.WriteLine("RecipeId being inserted: " + recipeId);
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

    public async Task UpdateAccess(Guid userId, Guid cartId, CartAccessType access)
    {
        CartAccess? a = await _applicationDbContext.CartAccesses.FirstOrDefaultAsync(a => a.UserId ==  userId && a.CartId==cartId);
        if (a == null) return;

        a.AccessType = access;
        _applicationDbContext.CartAccesses.Update(a);
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
        return await _applicationDbContext.Carts.AsSplitQuery()
            .Include(c=>c.Recipes)
                .ThenInclude(cr=>cr.Recipe)
            .Include(c=>c.Accesses)
                .ThenInclude(ca => ca.User)
            .Include(c=>c.Owner).FirstOrDefaultAsync(c => c.Id == cartId && c.OwnerId == userId);
    }


    /// <summary>
    /// Though this violates the clean architecture it allows to avoid the include explosion, hence the decision to loosen up the clean arch. constraints
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cartId"></param>
    /// <returns></returns>
    public async Task<CartResponse?> GetCartDetailsAsync(Guid userId, Guid cartId)
    {
        return await _applicationDbContext.Carts
            .Where(c => c.Id == cartId && c.Accesses.Any(a=>a.UserId==userId))
            .Select(c => new CartResponse
            {
                CartId = c.Id,
                OwnerUserName = c.Owner.UserName!,
                Members = c.Accesses.Select(a => new CartAccessDTO
                {
                    UserName = a.User.UserName!,
                    AccessType = a.AccessType
                }).ToList(),
                CartRecipes = c.Recipes.Select(r => new CartRecipeResponse
                {
                    Quantity = r.Quantity,
                    Recipe = new RecipeResponse
                    {
                        ExternalId = r.Recipe.ExternalId,
                        Name = r.Recipe.RecipeName,
                        Category = r.Recipe.Category.CategoryName,
                        Area = r.Recipe.Area,
                        Instructions = r.Recipe.Instructions,
                        ImageUrl = r.Recipe.ImageUrl,
                        Ingredients = r.Recipe.RecipeIngredients.Select(i => new IngredientDTO()
                        {
                            IngredientMeasure = i.Measurement,
                            IngredientName = i.Ingredient.Name,
                        }).ToList()
                    }
                }).ToList()
            })
            .FirstOrDefaultAsync();
    }

    public async Task<List<CartRecipe>> GetCartRecipes(Guid userId, Guid cartId)
    {
        Cart? c = await GetCartByIdAsync(userId, cartId);
        return c?.Recipes.ToList()?? new List<CartRecipe>();
    }

    public async Task<List<Cart>> GetAccessibleCartsAsync(Guid userId)
    {
        List<CartAccess> accesses =  await _applicationDbContext.CartAccesses.AsSplitQuery().Where(c => c.UserId == userId && c.AccessType!=CartAccessType.Owner && c.AccessType!=CartAccessType.Revoked)
                                                .GroupBy(c=>c.CartId).Select(c=>c.First()).ToListAsync();

        List<Guid> ids = accesses.Select(a => a.CartId).ToList();

        List<Cart> carts = await _applicationDbContext.Carts.Include(c => c.Owner).Where(c => ids.Contains(c.Id)).ToListAsync();

        return carts;
    }

    public async Task<List<Cart>> GetOwnedCartsAsync(Guid userId)
    {
        //List<CartAccess>? accesses = await _applicationDbContext.CartAccesses.Where(c => c.UserId == userId && c.AccessType==CartAccessType.Owner)
        //                                                                     .GroupBy(c => c.CartId)
        //                                                                     .Select(g => g.First())
        //                                                                     .Distinct().ToListAsync();
        //List<Cart>? carts = new List<Cart>();

        //foreach (var c in accesses)
        //{
        //    carts.Add(c.Cart);
        //}
        //return carts;
        Console.WriteLine("Cart created for: " + userId);

        return await _applicationDbContext.Carts
        .Where(c => c.OwnerId == userId)
        .ToListAsync();
    }

    public async Task RemoveFromCartAsync(Guid userId, Guid cartId, Guid recipeId)
    {
        Cart? c = await GetCartByIdAsync(userId, cartId);
        if (c == null)
        {
            _logger.LogWarning($"null cart {cartId}, {userId}");
            return;
        }
        CartRecipe? r = c?.Recipes?.FirstOrDefault(r => r.RecipeId == recipeId);
        if (r == null)
        {
            _logger.LogWarning("null recipe");
            return;
        }
        if (r.Quantity == 1)
            _applicationDbContext.CartRecipeMappings.Remove(r);
        else
        {
            _logger.LogWarning("Reducing quantity");
            r.Quantity--;
            _applicationDbContext.Update(r);
        }    
            //_applicationDbContext.Carts.Remove(c);
        await _applicationDbContext.SaveChangesAsync();
    }


    public async Task<bool?> HasPermission(Guid userId, Guid cartId, CartAccessType access)
    {
        CartAccessType? a = (await _applicationDbContext.CartAccesses.FirstOrDefaultAsync(ca => ca.CartId == cartId && ca.UserId==userId))?.AccessType;
        return a.HasValue && a.Value <= access;
    }
    public async Task ClearCart(Guid cartId)
    {
        Cart? c = await _applicationDbContext.Carts.FirstOrDefaultAsync(c => c.Id == cartId);
        if (c == null) return;

        c.Recipes.Clear();
        _applicationDbContext.Carts.Update(c);
        await _applicationDbContext.SaveChangesAsync();
    }
}
