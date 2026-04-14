using ArtAuction.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArtAuction.Infrastructure.Persistence.Configurations;

public class ArtistConfigurations : IEntityTypeConfiguration<Artist>
{
    public void Configure(EntityTypeBuilder<Artist> builder)
    {
        // Set table name
        builder.ToTable("Artists");
        
        // Primary key
        builder.HasKey(artist => artist.Id);
        builder.Property(artist => artist.Id).UseIdentityColumn(seed: 1, increment: 1);

        // Other properties configurations
        builder.Property(artist => artist.UserId).IsRequired();
        builder.Property(artist => artist.PhoneNumber).HasMaxLength(13).IsRequired();
        builder.Property(artist => artist.City).HasMaxLength(50).IsRequired();
        builder.Property(artist => artist.Country).HasMaxLength(50).IsRequired();

        // Relationships configurations
        builder.HasOne(artist => artist.Admin).WithMany(admin => admin.Artist)
            .HasForeignKey(artist => artist.AdminId).OnDelete(DeleteBehavior.Restrict);
    }
}