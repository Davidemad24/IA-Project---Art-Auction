using ArtAuction.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArtAuction.Infrastructure.Persistence.Configurations;

public class TagConfigurations : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        // Set table name
        builder.ToTable("Tags");
        
        // Primary key
        builder.HasKey(tag => tag.Id);
        builder.Property(tag => tag.Id).UseIdentityColumn(seed: 1, increment: 1);
        
        // Other properties configurations
        builder.Property(tag => tag.Name).HasMaxLength(50).IsRequired();
        
        // Relationships
        builder.HasMany(tag => tag.PostTags).WithOne(postTag => postTag.Tag)
            .HasForeignKey(postTag => postTag.TagId).OnDelete(DeleteBehavior.Restrict);
    }
}