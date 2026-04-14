using ArtAuction.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArtAuction.Infrastructure.Persistence.Configurations;

public class AdminConfigurations : IEntityTypeConfiguration<Admin>
{
    public void Configure(EntityTypeBuilder<Admin> builder)
    {
        // Set table name
        builder.ToTable("Admins");
        
        // Primary key
        builder.HasKey(admin => admin.Id);
        builder.Property(admin => admin.Id).UseIdentityColumn(seed: 100001, increment: 1);
        
        // Other properties configurations
        builder.Property(admin => admin.UserId).IsRequired();
    }
}