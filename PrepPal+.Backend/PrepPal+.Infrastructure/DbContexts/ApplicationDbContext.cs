using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PrepPal_.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepPal_.Infrastructure.DbContexts;

public class ApplicationDbContext: IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) { }
    public virtual DbSet<ApplicationGroup> ApplicationGroups { get; set; }
    public virtual DbSet<GroupMembership> GroupMemberships { get; set; }

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
    }
}
