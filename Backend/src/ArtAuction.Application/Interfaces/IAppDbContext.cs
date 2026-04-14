using ArtAuction.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ArtAuction.Application.Interfaces;

public interface IAppDbContext
{
    DbSet<Buyer> Buyers { get; }
    DbSet<Artist> Artists { get; }
    DbSet<Admin> Admins { get; }
    DbSet<PostBid> PostBids { get; }
    DbSet<PostSold> PostSolds { get; }
    DbSet<ArtworkPost> ArtworkPosts { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
}
