using ArtAuction.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArtAuction.Infrastructure.Persistence.Configurations;

public class ArtworkPostConfigurations : IEntityTypeConfiguration<ArtworkPost>
{
    public void Configure(EntityTypeBuilder<ArtworkPost> builder)
    {
        // Set table name
        builder.ToTable("ArtworkPosts");
        
        // Primary key
        builder.HasKey(artworkPost => artworkPost.Id); 
        builder.Property(artworkPost => artworkPost.Id).UseIdentityColumn(seed: 1, increment: 1);
        
        // Other properties configurations
        builder.Property(artworkPost => artworkPost.Title).HasMaxLength(100).IsRequired(); 
        builder.Property(artworkPost => artworkPost.Description).HasMaxLength(300);
        builder.Property(artworkPost => artworkPost.InitialPrice).HasPrecision(18, 2).IsRequired();
        builder.Property(artworkPost => artworkPost.BuyNewPrice).HasPrecision(18, 2);
        builder.Property(artworkPost => artworkPost.StartDate).IsRequired();
        builder.Property(artworkPost => artworkPost.EndDate).IsRequired();
        builder.Property(artworkPost => artworkPost.Image).IsRequired();
        builder.Property(artworkPost => artworkPost.ArtistId).IsRequired();
        builder.Property(artworkPost => artworkPost.CategoryId).IsRequired();

        // Relationships configurations
        builder.HasOne(artworkPost => artworkPost.Category).WithMany(category => category.ArtworkPosts)
            .HasForeignKey(artworkPost => artworkPost.CategoryId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(artworkPost => artworkPost.Artist).WithMany(artist => artist.ArtworkPosts)
            .HasForeignKey(artworkPost => artworkPost.ArtistId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(artworkPost => artworkPost.Admin).WithMany(admin => admin.ArtworkPosts)
            .HasForeignKey(artworkPost => artworkPost.AdminId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(artworkPost => artworkPost.PostBids).WithOne(postBid => postBid.ArtworkPost)
            .HasForeignKey(postBid => postBid.ArtworkPostId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(artworkPost => artworkPost.WatchLists).WithOne(watchList => watchList.ArtworkPost)
            .HasForeignKey(watchList => watchList.ArtworkPostId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(artworkPost => artworkPost.PostSolds).WithOne(postSold => postSold.ArtworkPost)
            .HasForeignKey(postSold => postSold.ArtworkPostId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(artworkPost => artworkPost.PostTags).WithOne(postTag => postTag.ArtworkPost)
            .HasForeignKey(postTag => postTag.ArtworkPostId).OnDelete(DeleteBehavior.Cascade);
    }
}