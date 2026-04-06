using ArtAuction.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArtAuction.Infrastructure.Persistence.Configurations;

public class PostTagConfigurations : IEntityTypeConfiguration<PostTag>
{
    public void Configure(EntityTypeBuilder<PostTag> builder)
    {
        // Set table name
        builder.ToTable("PostTags");
        
        // Primary key
        builder.HasKey(postTag => new { postTag.ArtworkPostId, postTag.TagId });
    }
}