using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Spectra.Domain.Entities;
using System.Text.RegularExpressions;

namespace Spectra.Infrastructure.Data;

/// <summary>
/// Database context for the Spectra application
/// </summary>
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Images uploaded to the platform
    /// </summary>
    public DbSet<Image> Images { get; set; }

    /// <summary>
    /// Follow relationships between users
    /// </summary>
    public DbSet<Follow> Follows { get; set; }

    /// <summary>
    /// Likes on images
    /// </summary>
    public DbSet<Like> Likes { get; set; }

    /// <summary>
    /// Comments on images
    /// </summary>
    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configure PostgreSQL naming conventions (snake_case)
        foreach (var entity in builder.Model.GetEntityTypes())
        {
            entity.SetTableName(entity.GetTableName()?.ToSnakeCase());

            foreach (var property in entity.GetProperties())
            {
                property.SetColumnName(property.GetColumnName().ToSnakeCase());
            }

            foreach (var key in entity.GetKeys())
            {
                key.SetName(key.GetName()?.ToSnakeCase());
            }

            foreach (var foreignKey in entity.GetForeignKeys())
            {
                foreignKey.SetConstraintName(foreignKey.GetConstraintName()?.ToSnakeCase());
            }

            foreach (var index in entity.GetIndexes())
            {
                index.SetDatabaseName(index.GetDatabaseName()?.ToSnakeCase());
            }
        }

        // Configure Image entity
        builder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.ImageUrl).IsRequired();
            entity.Property(e => e.UserId).IsRequired();

            // Configure relationship with User
            entity.HasOne(e => e.User)
                  .WithMany(u => u.Images)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Configure indexes
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.CreatedAt);
        });

        // Configure Follow entity
        builder.Entity<Follow>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FollowerId).IsRequired();
            entity.Property(e => e.FollowingId).IsRequired();

            // Configure relationships
            entity.HasOne(e => e.Follower)
                  .WithMany(u => u.Following)
                  .HasForeignKey(e => e.FollowerId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Following)
                  .WithMany(u => u.Followers)
                  .HasForeignKey(e => e.FollowingId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Ensure unique follow relationships
            entity.HasIndex(e => new { e.FollowerId, e.FollowingId }).IsUnique();
        });

        // Configure Like entity
        builder.Entity<Like>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UserId).IsRequired();
            entity.Property(e => e.ImageId).IsRequired();

            // Configure relationships
            entity.HasOne(e => e.User)
                  .WithMany(u => u.Likes)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Image)
                  .WithMany(i => i.Likes)
                  .HasForeignKey(e => e.ImageId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Ensure unique likes per user per image
            entity.HasIndex(e => new { e.UserId, e.ImageId }).IsUnique();
        });

        // Configure Comment entity
        builder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Content).IsRequired().HasMaxLength(500);
            entity.Property(e => e.UserId).IsRequired();
            entity.Property(e => e.ImageId).IsRequired();

            // Configure relationships
            entity.HasOne(e => e.User)
                  .WithMany(u => u.Comments)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Image)
                  .WithMany(i => i.Comments)
                  .HasForeignKey(e => e.ImageId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Configure indexes
            entity.HasIndex(e => e.ImageId);
            entity.HasIndex(e => e.CreatedAt);
        });

        // Configure ApplicationUser additional properties
        builder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(e => e.DisplayName).HasMaxLength(100);
            entity.Property(e => e.Bio).HasMaxLength(500);
            entity.Property(e => e.AvatarUrl).HasMaxLength(500);
        });
    }
}

/// <summary>
/// Extension methods for string manipulation
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Converts PascalCase to snake_case
    /// </summary>
    public static string ToSnakeCase(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        return Regex.Replace(input, "([a-z0-9])([A-Z])", "$1_$2").ToLower();
    }
}
