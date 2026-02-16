using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrepPal_.Core.Domain.Entities.RecipeEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Infrastructure.EntityConfig.RecipeConfig;

internal class UserRecipeInteractionConfiguration : IEntityTypeConfiguration<UserRecipeInteraction>
{
    public void Configure(EntityTypeBuilder<UserRecipeInteraction> builder)
    {
        builder.HasKey(x => new { x.UserId, x.RecipeId, x.Type });

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Recipe)
            .WithMany()
            .HasForeignKey(x => x.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.Type).HasConversion<int>();

        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => x.ExternalRecipeId);
    }
}
