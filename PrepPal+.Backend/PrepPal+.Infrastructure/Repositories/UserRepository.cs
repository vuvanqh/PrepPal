using Microsoft.EntityFrameworkCore;
using PrepPal_.Core.Domain.Entities;
using PrepPal_.Core.Domain.RepositoryContracts;
using PrepPal_.Infrastructure.DbContexts;

namespace PrepPal_.Infrastructure;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _applicationDbContext;
    public UserRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<ApplicationUser> GetUserByRefreshToken(string tokenHash)
    {
        return (await _applicationDbContext.Users.FirstOrDefaultAsync(user => user.TokenHash == tokenHash))!;
    }
    public async Task<ApplicationUser> GetUserById(Guid id) => (await _applicationDbContext.Users.FirstOrDefaultAsync(user => user.Id == id))!;
}
