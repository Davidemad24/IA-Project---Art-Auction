using ArtAuction.Application.Entities;
using ArtAuction.Application.Interfaces.Repositories;
using ArtAuction.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ArtAuction.Infrastructure.Repositories;

public class PostSoldRepo : IPostSoldRepo
{
    // Attributes
    private readonly AppDbContext _dbContext;
    private readonly ISaveChanges _saveChanges;
    
    // Constructor
    public PostSoldRepo(AppDbContext dbContext, ISaveChanges saveChanges)
    {
        this._dbContext = dbContext;
        this._saveChanges = saveChanges;
    }
    
    // Methods
    public async Task<ICollection<PostSold>> GetAllPostSolds()
    {
        return await _dbContext.PostSolds
            .Include(postSold => postSold.ArtworkPost)
            .ThenInclude(artworkPost => artworkPost.Category)
            .Include(postSold => postSold.Buyer).ToListAsync();
    }

    public async Task<PostSold?> GetUnpaidPostSoldByBuyer(int buyerId)
    {
        return await _dbContext.PostSolds
            .Include(postSold => postSold.ArtworkPost)
            .FirstOrDefaultAsync(postSold => postSold.BuyerId == buyerId && !postSold.IsPaid);
    }

    public async Task<bool> CreatePostSold(PostSold postSold)
    {
        // Check existence
        var post = await _dbContext.PostSolds.FirstOrDefaultAsync(
                sp => sp.ArtworkPostId == postSold.ArtworkPostId &&
                sp.BuyerId == postSold.BuyerId
            );
        if (post != null)
            return false;
        
        // Add postSold
        await _dbContext.PostSolds.AddAsync(postSold);
        return await _saveChanges.Save();
    }

    public async Task<bool> UpdatePostBuyer(PostSold postSold)
    {
        // Check existence
        var post = await _dbContext.PostSolds.FirstOrDefaultAsync(sp => sp.ArtworkPostId == postSold.ArtworkPostId);
        if (post == null)
            return false;
        
        // Update buyer
        post.BuyerId = postSold.BuyerId;
        return await _saveChanges.Save();
    }

    public async Task<bool> MarkAsPaid(int artworkPostId, int buyerId)
    {
        // Check existence
        var postSold = await _dbContext.PostSolds
            .FirstOrDefaultAsync(ps => ps.ArtworkPostId == artworkPostId && ps.BuyerId == buyerId);
        if (postSold == null)
            return false;

        // Update attribute
        postSold.IsPaid = true;
        return await _saveChanges.Save();
    }
}