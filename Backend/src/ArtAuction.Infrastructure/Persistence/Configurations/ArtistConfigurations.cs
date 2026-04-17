using ArtAuction.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArtAuction.Infrastructure.Persistence.Configurations;

public class ArtistConfigurations : IEntityTypeConfiguration<Artist>
{
    public void Configure(EntityTypeBuilder<Artist> builder)
    {
        // Other properties configurations
        builder.Property(artist => artist.PhoneNumber).HasMaxLength(13);
        builder.Property(artist => artist.City).HasMaxLength(50);
        builder.Property(artist => artist.Country).HasMaxLength(50);

        // Relationships configurations
        builder.HasOne(artist => artist.Admin).WithMany(admin => admin.Artist)
            .HasForeignKey(artist => artist.AdminId).OnDelete(DeleteBehavior.Restrict);
    }
}