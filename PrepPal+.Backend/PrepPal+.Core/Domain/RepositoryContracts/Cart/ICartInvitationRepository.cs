using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Core.Domain;

public interface ICartInvitationRepository
{
    Task AddInvitation(CartInvitation invitation);
    Task UpdateInvitation(Guid invitationId, Status status);
    Task DeleteInvitation(Guid invitationId);
    Task<List<CartInvitation>> GetCartInvitationsByStatus(Guid cartId, Status status);
    Task<List<CartInvitation>> GetUserInvitation(Guid userId, Status status);
    Task<CartInvitation?> GetInvitationById(Guid id);

}
