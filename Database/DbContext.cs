using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using twizzle.Models;

namespace twizzle.Database;

public class TwizzleDbContext : DbContext
{
    public TwizzleDbContext(DbContextOptions<TwizzleDbContext> options)
        : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Twizz> Twizzs { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<Follow> Follows { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureWarnings(warnings =>
        {
            warnings.Ignore(CoreEventId.NavigationBaseIncludeIgnored);
        });
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(u => u.Id);
        modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

        modelBuilder.Entity<Twizz>().HasIndex(t => t.Id);
        modelBuilder.Entity<Comment>().HasIndex(c => c.Id);
        modelBuilder.Entity<Like>().HasIndex(l => l.Id);
        modelBuilder.Entity<Follow>().HasIndex(f => f.Id);

        // User -> Twizz relationship
        modelBuilder
            .Entity<User>()
            .HasMany(u => u.Twizzs)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.UserId);

        // User -> Comment relationship
        modelBuilder
            .Entity<User>()
            .HasMany(u => u.Comments)
            .WithOne(c => c.User)
            .HasForeignKey(c => c.UserId);

        // User -> Like relationship
        modelBuilder
            .Entity<User>()
            .HasMany(u => u.Likes)
            .WithOne(l => l.User)
            .HasForeignKey(l => l.UserId);

        // User -> Followers relationship
        modelBuilder
            .Entity<User>()
            .HasMany(u => u.Followers)
            .WithOne(f => f.Follower)
            .HasForeignKey(f => f.FollowerId)
            .OnDelete(DeleteBehavior.Cascade);

        // User -> Following relationship
        modelBuilder
            .Entity<User>()
            .HasMany(u => u.Followings)
            .WithOne(f => f.Following)
            .HasForeignKey(f => f.FollowingId)
            .OnDelete(DeleteBehavior.Cascade);

        // Twizz -> Comment relationship
        modelBuilder
            .Entity<Twizz>()
            .HasMany(t => t.Comments)
            .WithOne(c => c.Twizz)
            .HasForeignKey(c => c.TwizzId);

        // Twizz -> Likes relationship
        modelBuilder
            .Entity<Twizz>()
            .HasMany(t => t.Likes)
            .WithOne(l => l.Twizz)
            .HasForeignKey(l => l.LikedId);

        // Follower -> Follower
        modelBuilder
            .Entity<Follow>()
            .HasOne(f => f.Follower)
            .WithMany(u => u.Followers)
            .HasForeignKey(f => f.FollowerId)
            .OnDelete(DeleteBehavior.Cascade);

        // Followers -> Following
        modelBuilder
            .Entity<Follow>()
            .HasOne(f => f.Following)
            .WithMany(u => u.Followings)
            .HasForeignKey(f => f.FollowingId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
