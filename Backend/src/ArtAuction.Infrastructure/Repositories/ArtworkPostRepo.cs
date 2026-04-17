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
        return await _dbContext.ArtworkPosts.
            Where(artworkPost => artworkPost.EndDate > DateTime.UtcNow)
            .Include(artworkPost => artworkPost.Artist)
            .Include(artworkPost => artworkPost.Category)
            .Include(artworkPost => artworkPost.PostTags)
            .ThenInclude(postTag => postTag.Tag)
            .AsNoTracking().ToListAsync();
    }

    public async Task<ICollection<ArtworkPost>> GetAllArtworkPostsForArtist(int artistId)
    {
        return await _dbContext.ArtworkPosts.
             Where(
                artworkPost => artworkPost.ArtistId == artistId &&
                artworkPost.EndDate > DateTime.UtcNow
            ).Include(artworkPost => artworkPost.Category)
            .Include(artworkPost => artworkPost.PostTags)
            .ThenInclude(artworkPostTag => artworkPostTag.Tag)
            .AsNoTracking().ToListAsync();
    }

    public async Task<ArtworkPost?> GetArtworkPost(int artworkPostId)
    {
        return await _dbContext.ArtworkPosts.
            Where(
                artworkPost => artworkPost.AdminId != null &&
                artworkPost.EndDate > DateTime.UtcNow
            ).Include(artworkPost => artworkPost.Artist)
            .Include(artworkPost => artworkPost.Category)
            .Include(artworkPost => artworkPost.PostBids)
            .ThenInclude(postBids => postBids.Buyer)
            .Include(artworkPost => artworkPost.PostTags)
            .ThenInclude(artworkPostTag => artworkPostTag.Tag)
            .AsNoTracking().FirstOrDefaultAsync(
                artworkPost => artworkPost.Id == artworkPostId && artworkPost.AdminId == null
            );
    }

    public async Task<ICollection<ArtworkPost>> GetUnapprovedArtworkPosts()
    {
        return await _dbContext.ArtworkPosts.
            Where(artworkPost => artworkPost.AdminId == null)
            .Include(artworkPost => artworkPost.Artist)
            .Include(artworkPost => artworkPost.Category)
            .Include(artworkPost => artworkPost.PostTags)
            .ThenInclude(artworkPostTag => artworkPostTag.Tag)
            .AsNoTracking().ToListAsync();
    }

    public async Task<bool> CreateArtworkPost(ArtworkPost artworkPost)
    {
        // Check artist existence
        var isArtistExist = await _dbContext.Artists.AnyAsync(artist => artist.Id == artworkPost.ArtistId);
        if (!isArtistExist)
            return false;
        
        await _dbContext.ArtworkPosts.AddAsync(artworkPost);
        return await _saveChanges.Save();
    }

    public async Task<bool> UpdateArtworkPost(ArtworkPost artworkPost)
    {
        // Check existence
        var artpost = await _dbContext.ArtworkPosts
            .Include(art => art.PostTags)
            .FirstOrDefaultAsync(art => art.Id == artworkPost.Id);
        if (artpost == null)
            return false;
        
        // Update main fields
        _dbContext.Entry(artpost).CurrentValues.SetValues(artworkPost);
        
        // Clear past relation
        artpost.PostTags.Clear();
        
        // Add relationships
        foreach (var tag in artworkPost.PostTags)
        {
            artpost.PostTags.Add(new PostTag 
            { 
                ArtworkPostId = artpost.Id, 
                TagId = tag.TagId 
            });
        }
        
        // return stats
        return await _saveChanges.Save();
    }

    public async Task<bool> DeleteArtworkPost(int artworkPostId)
    {
        // Check existence
        var artworkPost = await _dbContext.ArtworkPosts
            .FirstOrDefaultAsync(artworkPost => artworkPost.Id == artworkPostId);
        if (artworkPost == null)
            return false;

        // Remove artworkPost and return stats
        _dbContext.ArtworkPosts.Remove(artworkPost);
        return await _saveChanges.Save();
    }

    public async Task<bool> UpdateEndDate(int artworkPostId, DateTime endDate)
    {
        // Check existence
        var artworkPost = await _dbContext.ArtworkPosts.FirstOrDefaultAsync(art => art.Id == artworkPostId);
        if (artworkPost == null) 
            return false;
        
        // Update end_date
        artworkPost.EndDate = endDate;
        return await _saveChanges.Save();
    }

    public async Task<bool> MarkAsApproved(int artworkPostId, int adminId)
    {
        // Check existence
        var artworkPost = await _dbContext.ArtworkPosts.FirstOrDefaultAsync(art => art.Id == artworkPostId);
        if (artworkPost == null)
            return false;

        // Check admin existence
        System.Console.WriteLine($"Marking artwork post {artworkPostId} as approved");
        var isAdminExist = await _dbContext.Admins.AnyAsync(admin => admin.Id == adminId);
        if (!isAdminExist)
            return false;
        
        // Update end_date
        artworkPost.AdminId = adminId;
        return await _saveChanges.Save();
    }
}