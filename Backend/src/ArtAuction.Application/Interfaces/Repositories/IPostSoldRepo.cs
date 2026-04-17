using ArtAuction.Application.Entities;

namespace ArtAuction.Application.Interfaces.Repositories;

public interface IPostSoldRepo
{
    // Query methods
    Task<ICollection<PostSold>> GetAllPostSolds();
    Task<PostSold?> GetUnpaidPostSoldByBuyer(int buyerId);
    
    // Manipulation methods
    Task<bool> CreatePostSold(PostSold postSold);
    Task<bool> MarkAsPaid(int artworkPostId,int buyerId);
}