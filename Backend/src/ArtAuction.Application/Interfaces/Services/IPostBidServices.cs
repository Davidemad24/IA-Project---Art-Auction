using ArtAuction.Application.DTOs.ArtworkPost;

namespace ArtAuction.Application.Interfaces.Services;

public interface IPostBidServices
{
    Task<bool> CreatePostBid(PostBidCreationDto postBidCreationDto);
    Task<bool> UpdatePostBid(PostBidCreationDto postBidUpdatingDto);
    Task<bool> DeletePostBid(int artworkPostId, int buyerId);
}