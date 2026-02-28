using PrepPal_.Core.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Core;

public interface IConnectionService
{
    Task<ConnectionResponse> AddConnectionRequest(Guid userId, string userName);
    Task<Guid> ModifyConnection(Guid userId, Guid connectionId, ActionType action);
    Task<List<ConnectionResponse>> GetAllFriends(Guid userId);
    Task<Guid> GetReceiverId(Guid userId, Guid connectionId);
    Task<Connection> CheckConnectionExistance(Guid userId, Guid connectionId);
    Task<List<UserReposnse>> SearchByUser(string search);
}
