using ArtAuction.Application.Entities;

namespace ArtAuction.Application.Interfaces.Repositories;

public interface IPostBidRepo
{
    // Query methods
    Task<List<PostBid>> GetAllPostBids(int arworkPostId);
    Task<PostBid?> GetTopBid(int artworkPostId);
    Task<ICollection<PostBid>> GetAllBuyerBids(int buyerId);
    
    // Manipulation methods
    Task<bool> CreatePostBid(PostBid postBid);
    Task<bool> DeletePostBid(int artworkPostId, int buyerId);
}