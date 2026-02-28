using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PrepPal_.Core;
using PrepPal_.Core.Domain;
using PrepPal_.Infrastructure.DbContexts;
using PrepPal_.Infrastructure.Migrations;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Infrastructure.Repositories;

public class CartInvitationRepository : ICartInvitationRepository
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly ILogger<CartRepository> _logger;

    public CartInvitationRepository(ApplicationDbContext applicationDbContext, ILogger<CartRepository> logger)
    {
        _applicationDbContext = applicationDbContext;
        _logger = logger;
    }
    public async Task AddInvitation(CartInvitation invitation)
    {
        await _applicationDbContext.CartInvitations.AddAsync(invitation);
        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task DeleteInvitation(Guid invitationId)
    {
        CartInvitation? i = await _applicationDbContext.CartInvitations.FirstOrDefaultAsync(c => c.Id == invitationId);
        if (i == null) return;

        _applicationDbContext.CartInvitations.Remove(i);
        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task<List<CartInvitation>> GetCartInvitationsByStatus(Guid cartId, Status status)
    {
        return await _applicationDbContext.CartInvitations.Where(i => i.CartId == cartId && i.Status == status).ToListAsync(); 
    }

    public async Task<List<CartInvitation>> GetUserInvitation(Guid userId, Status status)
    {
        return await _applicationDbContext.CartInvitations
            .Include(i => i.Cart).ThenInclude(c=>c.Owner)
            .Where(i => i.ReceiverId == userId && i.Status == status).ToListAsync();
    }

    public async Task UpdateInvitation(Guid invitationId, Status status)
    {
        CartInvitation? i = await _applicationDbContext.CartInvitations.FirstOrDefaultAsync(c => c.Id == invitationId);
        if (i == null) return;
        i.Status = status;
        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task<CartInvitation?> GetInvitationById(Guid id) => await _applicationDbContext.CartInvitations.FirstOrDefaultAsync(i => i.Id == id);
}
