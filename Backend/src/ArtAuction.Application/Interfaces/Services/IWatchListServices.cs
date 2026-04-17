using ArtAuction.Application.DTOs.ArtworkPost;
using ArtAuction.Application.DTOs.WatchList;

namespace ArtAuction.Application.Interfaces.Services;

public interface IWatchListServices
{
    Task<ICollection<ArtworkPostDto>> GetWatchListForBuyer(int buyerId);
    Task<bool> CreateWatchList(WatchListCreationDto watchListCreationDto);
    Task<bool> DeleteWatchList(int buyerId, int artworkPostId);
}
