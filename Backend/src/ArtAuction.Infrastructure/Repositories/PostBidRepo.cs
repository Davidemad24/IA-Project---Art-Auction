using ArtAuction.Application.Entities;
using ArtAuction.Application.Interfaces.Repositories;
using ArtAuction.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ArtAuction.Infrastructure.Repositories;

public class PostBidRepo : IPostBidRepo
{
    // Attributes
    private readonly AppDbContext _dbContext;
    private readonly ISaveChanges _saveChanges;
    
    // Constructor
    public PostBidRepo(AppDbContext dbContext, ISaveChanges saveChanges)
    {
        this._dbContext = dbContext;
        this._saveChanges = saveChanges;
    }

    public async Task<ICollection<PostBid>> GetAllPostBidsForPost(int postId)
    {
        // Return list of post bids
        return await _dbContext.PostBids.Where(postbid => postbid.ArtworkPostId == postId).ToListAsync();
    }

    public async Task<bool> CreatePostBid(PostBid postBid)
    {
        // Check existence
        var pb = await _dbContext.PostBids.FirstOrDefaultAsync(postbid =>
                postbid.ArtworkPostId == postBid.ArtworkPostId &&
                postbid.BuyerId == postBid.BuyerId
            );
        if (pb != null)
            return false;
        
        // Add post bid
        await _dbContext.PostBids.AddAsync(postBid);
        return await _saveChanges.Save();
    }

    public async Task<bool> UpdatePostBid(PostBid postBid)
    {
        // Check existence
        var pb = await _dbContext.PostBids.FirstOrDefaultAsync(postbid =>
            postbid.ArtworkPostId == postBid.ArtworkPostId &&
            postbid.BuyerId == postBid.BuyerId
        );
        if (pb == null)
            return false;
        
        // Update price
        pb.BuyerPrice = postBid.BuyerPrice;
        return await _saveChanges.Save();
    }

    public async Task<bool> DeletePostBid(int artworkPostId, int buyerId)
    {
        // Check existence
        var postBid = await _dbContext.PostBids.FirstOrDefaultAsync(postbid =>
            postbid.ArtworkPostId == artworkPostId &&
            postbid.BuyerId == buyerId
        );
        if (postBid == null)
            return false;
        
        // Delete bid
        _dbContext.PostBids.Remove(postBid);
        return await _saveChanges.Save();
    }
}