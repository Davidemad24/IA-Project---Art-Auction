using ArtAuction.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArtAuction.Infrastructure.Persistence.Configurations;

public class PostSoldConfigurations : IEntityTypeConfiguration<PostSold>
{
    public void Configure(EntityTypeBuilder<PostSold> builder)
    {
        // Set table name
        builder.ToTable("PostSolds");
        
        // Primary key
        builder.HasKey(postSold => new { postSold.BuyerId, postSold.ArtworkPostId });
        
        // Other properties configurations
        builder.Property(postSold => postSold.FinalPrice).IsRequired();
    }
}