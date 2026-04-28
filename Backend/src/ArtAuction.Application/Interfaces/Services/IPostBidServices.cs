using ArtAuction.Application.DTOs.ArtworkPost;
using ArtAuction.Application.Entities;

namespace ArtAuction.Application.Interfaces.Services;

public interface IPostBidServices
{
    Task<List<PostBidDto>> GetAllPostBids(int artworkPostId);
    Task<ICollection<BuyerPostBidDto>> GetAllBuyerBids(int buyerId);
    Task<bool> CreatePostBid(PostBidCreationDto postBidCreationDto);
    Task<bool> DeletePostBid(int artworkPostId, int buyerId);
}