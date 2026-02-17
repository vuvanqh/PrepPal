using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using PrepPal_.Core.Domain.Entities;
using PrepPal_.Core.Domain.Entities.RecipeEntities;
using PrepPal_.Core.Domain.RepositoryContracts;
using PrepPal_.Infrastructure.DbContexts;

namespace PrepPal_.Infrastructure;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IRecipeRepository _recipeRepository;
    public UserRepository(ApplicationDbContext applicationDbContext, IRecipeRepository recipeRepository)
    {
        _applicationDbContext = applicationDbContext;
        _recipeRepository = recipeRepository;
    }

    public async Task<ApplicationUser> GetUserByRefreshToken(string tokenHash)
    {
        return (await _applicationDbContext.Users.FirstOrDefaultAsync(user => user.TokenHash == tokenHash))!;
    }
    public async Task<ApplicationUser?> GetUserById(Guid id) => (await _applicationDbContext.Users.FirstOrDefaultAsync(user => user.Id == id));

    public async Task<List<Recipe>?> GetLikedRecipes(Guid id)
    {
        List<UserRecipeInteraction> interactions = await _applicationDbContext.UserRecipeInteractions.Where(u => u.UserId == id && u.Type == InteractionType.Like).ToListAsync();
        List<Recipe> recipes = new List<Recipe>();
        foreach (var interaction in interactions)
        {
            Recipe? recipe = await _recipeRepository.GetRecipeAsync(interaction.ExternalRecipeId);
            if (recipe!=null)
                recipes.Add(recipe);
        }
        return recipes;
    }
}
