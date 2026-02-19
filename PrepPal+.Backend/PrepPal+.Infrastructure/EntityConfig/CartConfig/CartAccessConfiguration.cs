using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrepPal_.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Infrastructure.EntityConfig.RecipeConfig;


public class CartAccessConfiguration : IEntityTypeConfiguration<CartAccess>
{
    public void Configure(EntityTypeBuilder<CartAccess> builder)
    {
        builder.ToTable(nameof(CartAccess));

        builder.HasKey(c => new {c.CartId, c.UserId});

        builder.HasOne(c => c.Cart)
            .WithMany(cr=> cr.Accesses)
            .HasForeignKey(c => c.CartId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.User)
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasIndex(x => x.CartId);
    }
}
