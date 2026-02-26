using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrepPal_.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Infrastructure;

public class CartInvitationConfiguration : IEntityTypeConfiguration<CartInvitation>
{
    public void Configure(EntityTypeBuilder<CartInvitation> builder)
    {
        builder.HasKey(c => c.Id);

        builder.HasOne(c => c.Receiver)
            .WithMany()
            .HasForeignKey(c => c.ReceiverId)
             .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Sender)
            .WithMany()
            .HasForeignKey(c => c.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Cart)
            .WithMany()
            .HasForeignKey(c => c.CartId);

        builder.HasIndex(c => c.ReceiverId);
        builder.HasIndex(c => c.SenderId);
        builder.HasIndex(c => c.CartId);
    }
}
