using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrepPal_.Core;
using PrepPal_.Core.Domain.Entities.RecipeEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Infrastructure.EntityConfig;

public class ConnectionConfiguration : IEntityTypeConfiguration<Connection>
{
    public void Configure(EntityTypeBuilder<Connection> builder)
    {
        builder.HasKey(c => c.Id);

        builder.HasOne(c => c.User1)
            .WithMany()
            .HasForeignKey(u => u.UserId1)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.User2)
            .WithMany()
            .HasForeignKey(u => u.UserId2)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(c => c.UserId1);
        builder.HasIndex(c => c.UserId2);
        builder.HasIndex(c => c.Status);
        builder.HasIndex(c => new { c.UserId1, c.UserId2 }).IsUnique();
    }
}