using Microsoft.EntityFrameworkCore;
using PrepPal_.Core;
using PrepPal_.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Infrastructure.Repositories;

public class ConnectionRepository : IConnectionRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public ConnectionRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task UpdateConnectionAsync(Connection c)
    {
        _applicationDbContext.Connections.Update(c);
        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task AddConnectionAsync(Connection c)
    {
        await _applicationDbContext.Connections.AddAsync(c);
        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task DeleteConnectionAsync(Connection c)
    {
        _applicationDbContext.Connections.Remove(c);
        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task<List<Connection>> GetConnectionsByTypeAsync(Guid userId, Status type)
    {

        if (!(await _applicationDbContext.Connections.AnyAsync(c => c.UserId1 == userId || c.UserId2 == userId))) 
            return new List<Connection>();

        return _applicationDbContext.Connections
            .Include(c=>c.User1)
            .Include(c=>c.User2)
            .Where(c => (c.UserId1 == userId || c.UserId2==userId)
                                                       && c.Status == type).ToList();
    }
    public Connection CreateConnection(Guid sender, Guid userId2)
    {
        if(sender == userId2) throw new InvalidOperationException("You cannot add yourself");
        Guid userId1 = sender;
        EnsureId(ref userId1, ref userId2);

        return new Connection()
        {
            Id = Guid.NewGuid(),
            UserId1 = userId1,
            UserId2 = userId2,
            Status = Status.Pending,
            RequestedByUserId = sender
        };
    }

    private void EnsureId(ref Guid userId1, ref Guid userId2)
    {
        if (userId1 > userId2)
        {
            Guid temp = userId1;
            userId1 = userId2;
            userId2 = temp;
        }
    }

    public async Task<bool> ConnectionExists(Guid userId1, Guid userId2)
    {
        EnsureId(ref userId1, ref userId2);

        return await _applicationDbContext.Connections.AnyAsync(con => userId1 == con.UserId1 && userId2 == con.UserId2);
    }

    public async Task<Connection?> GetConnectionAsync(Guid userId1, Guid userId2)
    {
        EnsureId(ref userId1, ref userId2);
        return await _applicationDbContext.Connections
            .Include(c => c.User1)
            .Include(c => c.User2)
            .FirstOrDefaultAsync(c => userId1 == c.UserId1 && userId2 == c.UserId2);
    }
    public async Task<Connection?> GetConnectionByIdAsync(Guid connectionId)
    {
        return await _applicationDbContext.Connections
            .Include(c=>c.User1)
            .Include(c=>c.User2)
            .FirstOrDefaultAsync(c => c.Id == connectionId);
    }

    public async Task<List<Connection>> GetAllConnectionsAsync(Guid userId)
    {
        if (!(await _applicationDbContext.Connections.AnyAsync(c => c.UserId1 == userId || c.UserId2 == userId)))
            return new List<Connection>();

        return _applicationDbContext.Connections
            .Include(c => c.User1)
            .Include(c => c.User2)
            .Where(c => c.UserId1 == userId || c.UserId2 == userId).ToList();
    }
}
