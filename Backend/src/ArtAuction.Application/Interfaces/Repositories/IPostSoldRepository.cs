using ArtAuction.Application.Entities;

namespace ArtAuction.Application.Interfaces.Repositories;

public interface IPostSoldRepository
{
    Task<PostSold?> GetByArtworkPostIdAsync(int artworkPostId);
    Task<IEnumerable<PostSold>> GetByBuyerIdAsync(int buyerId);
    Task AddAsync(PostSold postSold);
    Task UpdateAsync(PostSold postSold);
    Task SaveChangesAsync();
}