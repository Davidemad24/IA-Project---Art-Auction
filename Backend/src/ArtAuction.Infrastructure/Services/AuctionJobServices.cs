using ArtAuction.Application.DTOs.ArtworkPost;
using ArtAuction.Application.DTOs.PostSold;
using ArtAuction.Application.Interfaces.Services;

namespace ArtAuction.Infrastructure.Services;

public class AuctionJobServices : IAuctionJobServices
{
    // Attributes
    private readonly IAuctionServices _auctionServices;
    
    // Constructor
    public AuctionJobServices(IAuctionServices auctionServices)
    {
        _auctionServices = auctionServices;
    }
    
    // Method
    public async Task EndAuctionJob(int artworkPostId)
    {
        await _auctionServices.DetermineWinner(artworkPostId);
    }
}