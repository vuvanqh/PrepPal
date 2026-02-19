using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrepPal_.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Infrastructure.EntityConfig.RecipeConfig;


/// <summary>
/// perhaps for future extentions
/// </summary>
public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.ToTable("Carts");

        builder.HasKey(c => c.Id);
        
        builder.HasOne(c => c.Owner)
            .WithMany(u => u.Carts)
            .HasForeignKey(c=>c.OwnerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.OwnerId);
    }
}
