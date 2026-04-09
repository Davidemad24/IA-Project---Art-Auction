using ArtAuction.Application.Entities;

namespace ArtAuction.Application.Interfaces.Repositories;

public interface IPostBidRepo
{
    // Query
    Task<ICollection<PostBid>> GetAllPostBidsForPost(int postId);
    
    // Manipulation methods
    Task<bool> CreatePostBid(PostBid postBid);
    Task<bool> UpdatePostBid(PostBid postBid);
    Task<bool> DeletePostBid(int artworkPostId, int buyerId);
}