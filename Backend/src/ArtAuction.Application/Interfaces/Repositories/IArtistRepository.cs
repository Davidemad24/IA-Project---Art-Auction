using ArtAuction.Application.Entities;

namespace ArtAuction.Application.Interfaces.Repositories;

public interface IArtistRepository
{
    Task<Artist?> GetByIdAsync(int id);
    Task<Artist?> GetByUserIdAsync(int userId);
    Task<IEnumerable<Artist>> GetPendingArtistsAsync();  // Artists with no AdminId set yet = pending
    Task<IEnumerable<Artist>> GetAllArtistsAsync();
    Task AddAsync(Artist artist);
    Task UpdateAsync(Artist artist);
    Task DeleteAsync(Artist artist);
    Task SaveChangesAsync();
}