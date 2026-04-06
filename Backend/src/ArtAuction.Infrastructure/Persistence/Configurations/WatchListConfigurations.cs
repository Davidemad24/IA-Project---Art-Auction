using ArtAuction.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArtAuction.Infrastructure.Persistence.Configurations;

public class WatchListConfigurations : IEntityTypeConfiguration<WatchList>
{
    public void Configure(EntityTypeBuilder<WatchList> builder)
    {
        // Set table name
        builder.ToTable("WatchLists");
        
        // Primary key
        builder.HasKey(watchList => new { watchList.ArtworkPostId, watchList.BuyerId });
    }
}