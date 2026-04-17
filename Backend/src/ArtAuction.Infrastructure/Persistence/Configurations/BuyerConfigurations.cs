using ArtAuction.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArtAuction.Infrastructure.Persistence.Configurations;

public class BuyerConfigurations : IEntityTypeConfiguration<Buyer>
{
    public void Configure(EntityTypeBuilder<Buyer> builder)
    {
        // Other properties constraints
        builder.Property(buyer => buyer.City).HasMaxLength(50);
        builder.Property(buyer => buyer.Country).HasMaxLength(50);
        builder.Property(buyer => buyer.PhoneNumber).HasMaxLength(13);
        builder.Property(buyer => buyer.Address).HasMaxLength(150);
        
        // Relationships configurations
        builder.HasMany(buyer => buyer.WatchLists).WithOne(watchList => watchList.Buyer)
            .HasForeignKey(watchList => watchList.BuyerId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(buyer => buyer.PostBids).WithOne(postBid => postBid.Buyer)
            .HasForeignKey(postBid => postBid.BuyerId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(buyer => buyer.PostSolds).WithOne(postSold => postSold.Buyer)
            .HasForeignKey(postSold => postSold.BuyerId).OnDelete(DeleteBehavior.Restrict);
    }
}