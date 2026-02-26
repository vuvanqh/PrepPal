using PrepPal_.Core.Application.DTO.Social;
using PrepPal_.Core.Domain.Entities;
using PrepPal_.Core.Domain.RepositoryContracts;
using PrepPal_.Core.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Core;

public class ConnectionService : IConnectionService
{
    private readonly IConnectionRepository _connectionRepo;
    private readonly IUserRepository _userRepo;
    private readonly ConnectionCommandDispatcher _dispatcher;
    public ConnectionService(IConnectionRepository connecitonRepo, IUserRepository userRepo, ConnectionCommandDispatcher dispatcher)
    {
        _connectionRepo = connecitonRepo;
        _userRepo = userRepo;
        _dispatcher = dispatcher;
    }

    public async Task<ConnectionResponse> AddConnectionRequest(Guid userId, string userName)
    {
        ApplicationUser? user = await _userRepo.GetUserByUsernameAsync(userName);
        ApplicationUser requester = (await _userRepo.GetUserById(userId))!;
        if (user == null)
            throw new ArgumentException("User does not exist");

        if (await _connectionRepo.ConnectionExists(userId, user.Id))
            throw new InvalidOperationException("Connection already exists");

        Connection c = _connectionRepo.CreateConnection(userId, user.Id);

        await _connectionRepo.AddConnectionAsync(c);

        return new ConnectionResponse()
        {
            ConnectionId = c.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user.UserName!,
            RequestedByUsername = requester.UserName!,
            Status = c.Status
        };
    }

    public async Task<List<ConnectionResponse>> GetAllFriends(Guid userId)
    {
        List<Connection> connections =  await _connectionRepo.GetAllConnectionsAsync(userId);
        return connections.Select(c => {
            ApplicationUser u = c.UserId1 == userId ? c.User2 : c.User1;

            return new ConnectionResponse()
            {
                ConnectionId = c.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                UserName = u.UserName!,
                RequestedByUsername = c.RequestedByUserId==c.UserId2? c.User2.UserName!: c.User1.UserName!,
                Status = c.Status
            };
        }).ToList();
    }

    public async Task<Connection> CheckConnectionExistance(Guid userId, Guid connectionId)
    {
        Connection? c = await _connectionRepo.GetConnectionByIdAsync(connectionId);
        if (c == null || c.UserId1 != userId && c.UserId2 != userId)
            throw new ArgumentException("User yall are not friends");

        return c;
    }

    public async Task<Guid> GetReceiverId(Guid userId, Guid connectionId)
    {
        Connection c = await CheckConnectionExistance(userId, connectionId);
        return c.UserId1 == userId ? c.UserId2 : c.UserId1;
    }

    public async Task ModifyConnection(Guid userId, Guid connectionId, ActionType action)
    {
        Connection c = await CheckConnectionExistance(userId, connectionId);
        await _dispatcher.Dispatch(c, userId, action);

        if(action== ActionType.Cancel || action == ActionType.Remove || action == ActionType.Reject)
        {
            await _connectionRepo.DeleteConnectionAsync(c);
        }
        else if(action == ActionType.Accept)
        {
            await _connectionRepo.UpdateConnectionAsync(c);
        }
    }
    public async Task<List<UserReposnse>> SearchByUser(string search)
    {
        List<ApplicationUser> users = await _userRepo.FindUsersByUsername(search);
        return users.Select(u => new UserReposnse()
        {
            UserName = u.UserName!,
            FirstName = u.FirstName,
            LastName = u.LastName,
        }).ToList();
    }
}
