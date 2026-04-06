using ArtAuction.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArtAuction.Infrastructure.Persistence.Configurations;

public class CategoryConfigurations : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        // Set table name
        builder.ToTable("Categories");
        
        // Primary key
        builder.HasKey(category => category.Id);
        builder.Property(category => category.Id).UseIdentityColumn(seed: 1, increment: 1);
        
        // Other properties configurations
        builder.Property(category => category.Name).HasMaxLength(50).IsRequired();
    }
}