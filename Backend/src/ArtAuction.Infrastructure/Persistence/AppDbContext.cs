using ArtAuction.Application.Entities;
using ArtAuction.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ArtAuction.Infrastructure.Persistence;

public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Admin> Admins => Set<Admin>();
    public DbSet<Artist> Artists => Set<Artist>();
    public DbSet<Buyer> Buyers => Set<Buyer>();
    public DbSet<ArtworkPost> ArtworkPosts => Set<ArtworkPost>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<PostTag> PostTags => Set<PostTag>();
    public DbSet<PostBid> PostBids => Set<PostBid>();
    public DbSet<PostSold> PostSolds => Set<PostSold>();
    public DbSet<WatchList> WatchLists => Set<WatchList>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // ApplicationUser
        builder.Entity<ApplicationUser>(e =>
        {
            e.Property(u => u.FullName).IsRequired().HasMaxLength(100);
        });

        // Admin → User (1-to-1)
        builder.Entity<Admin>(e =>
        {
            e.HasKey(a => a.Id);
            e.HasOne<ApplicationUser>()
             .WithOne()
             .HasForeignKey<Admin>(a => a.UserId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // Artist → User (1-to-1), Artist → Admin (many-to-1)
        builder.Entity<Artist>(e =>
        {
            e.HasKey(a => a.Id);
            e.HasOne<ApplicationUser>()
             .WithOne()
             .HasForeignKey<Artist>(a => a.UserId)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(a => a.Admin)
             .WithMany(ad => ad.Artist)
             .HasForeignKey(a => a.AdminId)
             .OnDelete(DeleteBehavior.Restrict);
            e.Property(a => a.PhoneNumber).HasMaxLength(13);
        });

        // Buyer → User (1-to-1)
        builder.Entity<Buyer>(e =>
        {
            e.HasKey(b => b.Id);
            e.HasOne<ApplicationUser>()
             .WithOne()
             .HasForeignKey<Buyer>(b => b.UserId)
             .OnDelete(DeleteBehavior.Cascade);
            e.Property(b => b.PhoneNumber).HasMaxLength(13);
            e.Property(b => b.Address).HasMaxLength(150);
        });

        // ArtworkPost
        builder.Entity<ArtworkPost>(e =>
        {
            e.HasKey(p => p.Id);
            e.Property(p => p.Title).IsRequired().HasMaxLength(100);
            e.Property(p => p.Description).HasMaxLength(300);
            e.Property(p => p.InitialPrice).HasColumnType("decimal(18,2)");
            e.Property(p => p.BuyNewPrice).HasColumnType("decimal(18,2)");
            e.HasOne(p => p.Artist)
             .WithMany(a => a.ArtworkPosts)
             .HasForeignKey(p => p.ArtistId)
             .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(p => p.Admin)
             .WithMany(a => a.ArtworkPosts)
             .HasForeignKey(p => p.AdminId)
             .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(p => p.Category)
             .WithMany(c => c.ArtworkPosts)
             .HasForeignKey(p => p.CategoryId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // PostTag — composite PK
        builder.Entity<PostTag>(e =>
        {
            e.HasKey(pt => new { pt.ArtworkPostId, pt.TagId });
            e.HasOne(pt => pt.ArtworkPost)
             .WithMany(p => p.PostTags)
             .HasForeignKey(pt => pt.ArtworkPostId);
            e.HasOne(pt => pt.Tag)
             .WithMany(t => t.PostTags)
             .HasForeignKey(pt => pt.TagId);
        });

        // PostBid — composite PK
        builder.Entity<PostBid>(e =>
        {
            e.HasKey(pb => new { pb.BuyerId, pb.ArtworkPostId });
            e.Property(pb => pb.BuyerPrice).HasColumnType("decimal(18,2)");
            e.HasOne(pb => pb.Buyer)
             .WithMany(b => b.PostBids)
             .HasForeignKey(pb => pb.BuyerId)
             .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(pb => pb.ArtworkPost)
             .WithMany(p => p.PostBids)
             .HasForeignKey(pb => pb.ArtworkPostId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // PostSold — composite PK
        builder.Entity<PostSold>(e =>
        {
            e.HasKey(ps => new { ps.BuyerId, ps.ArtworkPostId });
            e.Property(ps => ps.FinalPrice).HasColumnType("decimal(18,2)");
            e.HasOne(ps => ps.Buyer)
             .WithMany(b => b.PostSolds)
             .HasForeignKey(ps => ps.BuyerId)
             .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(ps => ps.ArtworkPost)
             .WithMany(p => p.PostSolds)
             .HasForeignKey(ps => ps.ArtworkPostId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // WatchList — composite PK
        builder.Entity<WatchList>(e =>
        {
            e.HasKey(w => new { w.BuyerId, w.ArtworkPostId });
            e.HasOne(w => w.Buyer)
             .WithMany(b => b.WatchLists)
             .HasForeignKey(w => w.BuyerId)
             .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(w => w.ArtworkPost)
             .WithMany(p => p.WatchLists)
             .HasForeignKey(w => w.ArtworkPostId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // Category & Tag
        builder.Entity<Category>().Property(c => c.Name).IsRequired().HasMaxLength(50);
        builder.Entity<Tag>().Property(t => t.Name).IsRequired().HasMaxLength(50);
    }
}