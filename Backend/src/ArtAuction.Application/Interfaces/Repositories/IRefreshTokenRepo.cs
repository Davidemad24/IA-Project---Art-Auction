using ArtAuction.Application.Entities;

namespace ArtAuction.Application.Interfaces.Repositories;

public interface IRefreshTokenRepo
{
    // Query methods
    Task<RefreshToken?> GetAsync(string token);
    
    // Manipulation methods
    Task AddAsync(RefreshToken token);
    Task UpdateAsync(RefreshToken token);
}