using PrepPal_.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Core.Application.Services;

public class CartInvitationPolicy
{
    private readonly IConnectionRepository _connectionRepository;
    public CartInvitationPolicy(IConnectionRepository connectionRepository)
    {
        _connectionRepository = connectionRepository;
    }

    public async Task EnsureCanModifyStatus(Guid userId, CartInvitation invitation)
    {
        if(invitation.ReceiverId != userId) throw new UnauthorizedAccessException();

        List<Connection> c = await _connectionRepository.GetAllConnectionsAsync(userId);
        var con = c.FirstOrDefault(cn =>
        {
            ApplicationUser user = cn.UserId1 == invitation.SenderId ? cn.User2 : cn.User1;

            return invitation.ReceiverId==user.Id && cn.Status==Status.Accepted;
        });
        if (con == null)
            throw new InvalidOperationException("Invalid operation");
    } 
}
