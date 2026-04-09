using ArtAuction.Application.Entities;
using ArtAuction.Infrastructure.Identities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArtAuction.Infrastructure.Persistence.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasDiscriminator<string>("UserType")
            .HasValue<Artist>("Artist")
            .HasValue<Buyer>("Buyer")
            .HasValue<Admin>("Admin");
    }
}