using ArtAuction.Application.Entities;

namespace ArtAuction.Application.Interfaces.Repositories;

public interface IWatchListRepo
{
    // Query methods
    Task<ICollection<WatchList>> GetWatchListForBuyer(int buyerId);
    
    // Manipulation methods
    Task<bool> CreateWatchList(WatchList watchList);
    Task<bool> DeleteWatchList(int buyerId, int artworkPostId);
}
