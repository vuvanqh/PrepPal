
using PrepPal_.Core.Domain.Entities.RecipeEntities;
using Microsoft.EntityFrameworkCore;
using PrepPal_.Infrastructure.DbContexts;

public class IngredientRepository : IIngredientRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public IngredientRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }
    public async Task AddIngredientAsync(string ingredientName, string ingredientAlias)
    {
        if(await _applicationDbContext.Ingredients.AnyAsync(i => i.Name == ingredientName)) return;

        await _applicationDbContext.Ingredients.AddAsync(new Ingredient()
        {
            Id = Guid.NewGuid(),
            Name = ingredientName,
            AliasName = ingredientAlias
        });
        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task AddIngredientRepoMapping(Guid recipeId, Guid ingredientId, string measurement)
    {
        if(await _applicationDbContext.RecipeIngredients.AnyAsync(ir => ir.RecipeId==recipeId && ir.IngredientId == ingredientId))
            return;

        await _applicationDbContext.RecipeIngredients.AddAsync(new RecipeIngredient()
        {
            RecipeId = recipeId,
            IngredientId = ingredientId,
            Measurement =measurement
        });
        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task<Ingredient?> GetIngredientByName(string ingredientname)
    {
        return await _applicationDbContext.Ingredients.FirstOrDefaultAsync(i => i.Name == ingredientname);
    }

}