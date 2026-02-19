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
public class CartRecipeConfiguration : IEntityTypeConfiguration<CartRecipe>
{
    public void Configure(EntityTypeBuilder<CartRecipe> builder)
    {
        builder.ToTable("CartRecipeMappings");

        builder.HasKey(c => new {c.CartId, c.RecipeId});

        builder.HasOne(c => c.Cart)
            .WithMany(c=>c.Recipes)
            .HasForeignKey(c => c.CartId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.Recipe)
            .WithMany(c=>c.CartRecipes)
            .HasForeignKey(c => c.RecipeId) 
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.CartId);
    }
}
