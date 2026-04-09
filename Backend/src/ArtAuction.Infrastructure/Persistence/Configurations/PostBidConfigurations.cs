using ArtAuction.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArtAuction.Infrastructure.Persistence.Configurations;

public class PostBidConfigurations : IEntityTypeConfiguration<PostBid>
{
    public void Configure(EntityTypeBuilder<PostBid> builder)
    {
        // Set table name
        builder.ToTable("PostBids");
        
        // Primary key
        builder.HasKey(postBid => new { postBid.BuyerId, postBid.ArtworkPostId }); 
        
        // Other properties configurations
        builder.Property(postBid => postBid.BuyerPrice).HasPrecision(18, 2).IsRequired();
    }
}