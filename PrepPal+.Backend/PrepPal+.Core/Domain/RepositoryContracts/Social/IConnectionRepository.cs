using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Core;

public interface IConnectionRepository
{
    Task AddConnectionAsync(Connection c);
    Task DeleteConnectionAsync(Connection c);
    Task<List<Connection>> GetConnectionsByTypeAsync(Guid userId, Status type);
    Task<List<Connection>> GetAllConnectionsAsync(Guid userId);
    Task<bool> ConnectionExists(Guid userId1, Guid userId2);
    Task<Connection?> GetConnectionAsync(Guid userId1, Guid userId2);
    Task<Connection?> GetConnectionByIdAsync(Guid connectionId);
    Connection CreateConnection(Guid userId1, Guid userId2);
    Task UpdateConnectionAsync(Connection c);
}
