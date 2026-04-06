using ArtAuction.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArtAuction.Infrastructure.Persistence.Configurations;

public class BuyerConfigurations : IEntityTypeConfiguration<Buyer>
{
    public void Configure(EntityTypeBuilder<Buyer> builder)
    {
        // Set table name
        builder.ToTable("Buyers");
        
        // Primary Key
        builder.HasKey(buyer => buyer.Id);
        builder.Property(buyer => buyer.Id).UseIdentityColumn(seed: 1001, increment: 1);
        
        // Other properties constraints 
        builder.Property(buyer => buyer.UserId).IsRequired();
        builder.Property(buyer => buyer.City).HasMaxLength(50).IsRequired();
        builder.Property(buyer => buyer.Country).HasMaxLength(50).IsRequired();
        builder.Property(buyer => buyer.PhoneNumber).HasMaxLength(13).IsRequired();
        builder.Property(buyer => buyer.Address).HasMaxLength(150).IsRequired();
        
        // Relationships configurations
        builder.HasMany(buyer => buyer.WatchLists).WithOne(watchList => watchList.Buyer)
            .HasForeignKey(watchList => watchList.BuyerId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(buyer => buyer.PostBids).WithOne(postBid => postBid.Buyer)
            .HasForeignKey(postBid => postBid.BuyerId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(buyer => buyer.PostSolds).WithOne(postSold => postSold.Buyer)
            .HasForeignKey(postSold => postSold.BuyerId).OnDelete(DeleteBehavior.Restrict);
    }
}