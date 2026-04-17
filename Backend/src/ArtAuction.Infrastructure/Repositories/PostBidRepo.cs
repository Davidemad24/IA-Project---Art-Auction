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

    // Methods
    public async Task<PostBid?> GetTopBid(int artworkPostId)
    {
        return await _dbContext.PostBids
            .Where(postBid => postBid.ArtworkPostId == artworkPostId)
            .OrderByDescending(postBid => postBid.BuyerPrice)
            .AsNoTracking().FirstOrDefaultAsync();
    }

    public async Task<ICollection<PostBid>> GetAllBuyerBids(int buyerId)
    {
        return await _dbContext.PostBids.Where(
                postBid => postBid.BuyerId == buyerId && 
                postBid.ArtworkPost.EndDate > DateTime.UtcNow
            ).Include(postBid => postBid.ArtworkPost).AsNoTracking().ToListAsync();
    }

    public async Task<bool> CreatePostBid(PostBid postBid)
    {
        // Check artwork post existence
        var isArtworkPostExist = await _dbContext.ArtworkPosts
            .AnyAsync(artworkPost => artworkPost.Id == postBid.ArtworkPostId && artworkPost.AdminId != null);
        if (!isArtworkPostExist)
            return false;
        
        // Check buyer existence
        var isBuyerExist = await _dbContext.Buyers.AnyAsync(buyer => buyer.Id == postBid.BuyerId);
        if (!isBuyerExist)
            return false;
        
        // Add post bid
        await _dbContext.PostBids.AddAsync(postBid);
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