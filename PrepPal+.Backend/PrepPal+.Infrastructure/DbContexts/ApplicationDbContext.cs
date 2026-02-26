using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PrepPal_.Core;
using PrepPal_.Core.Domain.Entities;
using PrepPal_.Core.Domain.Entities.RecipeEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Infrastructure.DbContexts;

public class ApplicationDbContext: IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) { }
    public virtual DbSet<ApplicationGroup> ApplicationGroups => Set<ApplicationGroup>();
    public virtual DbSet<GroupMembership> GroupMemberships => Set<GroupMembership>();

    //recipe
    public virtual DbSet<Ingredient> Ingredients => Set<Ingredient>();
    public virtual DbSet<Recipe> Recipes => Set<Recipe>();
    public virtual DbSet<RecipeCategory> RecipeCategories => Set<RecipeCategory>();
    public virtual DbSet<RecipeIngredient> RecipeIngredients => Set<RecipeIngredient>();
    public virtual DbSet<UserRecipeInteraction> UserRecipeInteractions => Set<UserRecipeInteraction>();
    public virtual DbSet<Cart> Carts => Set<Cart>();
    public virtual DbSet<CartAccess> CartAccesses => Set<CartAccess>();
    public virtual DbSet<CartRecipe> CartRecipeMappings => Set<CartRecipe>();

    //social
    public virtual DbSet<Connection> Connections => Set<Connection>();
    public virtual DbSet<Message> Messages => Set<Message>();
    public virtual DbSet<CartInvitation> CartInvitations => Set<CartInvitation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ApplicationGroup>().ToTable("ApplicationGroups");
        modelBuilder.Entity<ApplicationUser>().ToTable("Users");
        modelBuilder.Entity<GroupMembership>().ToTable("GroupMemberships");

        //Relationships
        modelBuilder.Entity<GroupMembership>().HasKey(x => new { x.GroupId, x.UserId });

        modelBuilder.Entity<GroupMembership>()
            .HasOne<ApplicationUser>(x => x.ApplicationUser)
            .WithMany()
            .HasForeignKey(x => x.UserId);

        modelBuilder.Entity<GroupMembership>()
            .HasOne<ApplicationGroup>(x => x.ApplicationGroup)
            .WithMany()
            .HasForeignKey(x => x.GroupId);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
