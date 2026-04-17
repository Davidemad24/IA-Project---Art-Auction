using ArtAuction.Application.DTOs.ArtworkPost;

namespace ArtAuction.Application.Interfaces.Services;

public interface IPostBidServices
{
    Task<ICollection<BuyerPostBidDto>> GetAllBuyerBids(int buyerId);
    Task<bool> CreatePostBid(PostBidCreationDto postBidCreationDto);
    Task<bool> DeletePostBid(int artworkPostId, int buyerId);
}