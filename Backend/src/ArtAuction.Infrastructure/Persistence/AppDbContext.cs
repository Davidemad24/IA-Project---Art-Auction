using ArtAuction.Application.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ArtAuction.Infrastructure.Persistence;

public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
{
    // Constructors
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
    
    // Configurations of DbContext
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Note: Identity base configurations are still applied via base call.
        base.OnModelCreating(modelBuilder);
        
        // Add fluent API configuration
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
    
    // Entities
    public DbSet<Artist> Artists { get; set; }
    public DbSet<Buyer> Buyers { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<ArtworkPost> ArtworkPosts { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<PostTag> PostTags { get; set; }
    public DbSet<PostSold> PostSolds { get; set; }
    public DbSet<PostBid> PostBids { get; set; }
    public DbSet<WatchList> WatchLists { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
}