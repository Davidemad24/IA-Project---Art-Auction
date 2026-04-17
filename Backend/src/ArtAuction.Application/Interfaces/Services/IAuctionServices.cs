using ArtAuction.Application.DTOs.PostSold;

namespace ArtAuction.Application.Interfaces.Services;

public interface IAuctionServices
{
    Task DetermineWinner(int artworkPostId);
}