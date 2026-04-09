using ArtAuction.Application.Entities;
using ArtAuction.Application.Interfaces.Repositories;
using ArtAuction.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ArtAuction.Infrastructure.Repositories;

public class ArtworkPostRepo : IArtworkPostRepo
{
    // Attributes
    private readonly AppDbContext _dbContext;
    private readonly ISaveChanges _saveChanges;
    
    // Constructor
    public ArtworkPostRepo(AppDbContext dbContext, ISaveChanges saveChanges)
    {
        this._dbContext = dbContext;
        this._saveChanges = saveChanges;
    }
    
    // Methods
    public async Task<ICollection<ArtworkPost>> GetAllArtworkPosts()
    {
        return await _dbContext.ArtworkPosts.Where(artwworkPost => artwworkPost.AdminId != null)
            .Include(artworkPost => artworkPost.Artist)
            .Include(artworkPost => artworkPost.Category).ToListAsync();
    }

    public async Task<ICollection<ArtworkPost>> GetAllArtworkPostsForArtist(int artistId)
    {
        return await _dbContext.ArtworkPosts
            .Where(artworkPost => artworkPost.ArtistId == artistId)
            .Include(artworkPost => artworkPost.Category).ToListAsync();
    }

    public async Task<ArtworkPost?> GetArtworkPost(int artworkPostId)
    {
        return await _dbContext.ArtworkPosts
            .Include(artworkPost => artworkPost.Artist)
            .Include(artworkPost => artworkPost.Category)
            .FirstOrDefaultAsync(artworkPost => artworkPost.Id == artworkPostId);
    }

    public async Task<ICollection<ArtworkPost>> GetUnapprovedArtworkPosts()
    {
        return await _dbContext.ArtworkPosts
            .Where(artworkPost => artworkPost.AdminId == null)
            .Include(artworkPost => artworkPost.Artist)
            .Include(artworkPost => artworkPost.Category).ToListAsync();
    }

    public async Task<bool> CreateArtworkPost(ArtworkPost artworkPost)
    {
        await _dbContext.ArtworkPosts.AddAsync(artworkPost);
        return await _saveChanges.Save();
    }

    public async Task<bool> UpdateArtworkPost(ArtworkPost artworkPost)
    {
        // Check existence
        var artpost = await _dbContext.ArtworkPosts
            .FirstOrDefaultAsync(art => art.Id == artworkPost.Id);
        if (artpost != null)
            return false;
        
        _dbContext.ArtworkPosts.Update(artworkPost);
        return await _saveChanges.Save();
    }

    public async Task<bool> DeleteArtworkPost(int artworkPostId)
    {
        // Check existence
        var artworkPost = await _dbContext.ArtworkPosts
            .FirstOrDefaultAsync(artworkPost => artworkPost.Id == artworkPostId);
        if (artworkPost != null)
            return false;

        // Remove artworkPost
        _dbContext.ArtworkPosts.Remove(artworkPost);

        // Save changes and return stats
        return await _saveChanges.Save();
    }

    public async Task<bool> UpdateEndDate(int artworkPostId, DateTime endDate)
    {
        // Check existence
        var awt = await _dbContext.ArtworkPosts.FirstOrDefaultAsync(art => art.Id == artworkPostId);
        if (awt == null) 
            return false;
        
        // Update end_date
        awt.EndDate = endDate;
        return await _saveChanges.Save();
    }

    public async Task<bool> MarkAsApproved(int artworkPostId, int adminId)
    {
        // Check existence
        var artworkPost = await _dbContext.ArtworkPosts.FirstOrDefaultAsync(art => art.Id == artworkPostId);
        if (artworkPost == null) 
            return false;
        
        // Update end_date
        artworkPost.AdminId = adminId;
        return await _saveChanges.Save();
    }
}