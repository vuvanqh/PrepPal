using Microsoft.EntityFrameworkCore;
using PrepPal_.Core;
using PrepPal_.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Infrastructure.Repositories.Social;

public class MessageRepository : IMessageRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public MessageRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }
    public async Task<List<Message>> GetConversationAsync(Guid conversationId)
    {
        if (!(await _applicationDbContext.Messages.AnyAsync(m => conversationId == m.ConnectionId))) return new List<Message>();

        List<Message> messages =  _applicationDbContext.Messages.Where(m => m.ConnectionId == conversationId).ToList();
        return messages;
    }

    public async Task SaveMessageAsync(Message message)
    {
        await _applicationDbContext.Messages.AddAsync(message);
        await _applicationDbContext.SaveChangesAsync();
    }
}
