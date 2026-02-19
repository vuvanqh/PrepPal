

using PrepPal_.Core.Domain.Entities.RecipeEntities;

public interface IIngredientRepository
{
    Task AddIngredientAsync(string ingredientName, string ingredientAlias);
    Task AddIngredientRepoMapping(Guid recipeId, Guid ingredientId, string measurement);
    Task<Ingredient?> GetIngredientByName(string ingredientname);
}