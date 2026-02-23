using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrepPal_.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Infrastructure.EntityConfig;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasKey(c => c.Id);

        builder.HasOne(c => c.Connection)
            .WithMany(c => c.Messages)
            .HasForeignKey(u => u.ConnectionId);

        builder.HasIndex(c => c.ConnectionId);
        builder.HasIndex(c => c.SenderUsername);
    }
}
