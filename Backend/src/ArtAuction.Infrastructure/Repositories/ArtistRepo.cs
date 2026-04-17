using ArtAuction.Application.Entities;
using ArtAuction.Application.Interfaces.Repositories;
using ArtAuction.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ArtAuction.Infrastructure.Repositories;

public class ArtistRepo : IArtistRepo
{
    // Attributes
    private readonly AppDbContext _dbContext;
    private readonly ISaveChanges _saveChanges;
    
    // Constructor
    public ArtistRepo(AppDbContext dbContext, ISaveChanges saveChanges)
    {
        this._dbContext = dbContext;
        this._saveChanges = saveChanges;
    }
    
    // Methods
    public async Task<Artist?> GetArtistById(int artistId)
    {
        return await _dbContext.Artists.AsNoTracking().FirstOrDefaultAsync(a => a.Id == artistId);
    }

    public async Task<List<Artist>> GetUnaprovedArtists()
    {
        return await _dbContext.Artists.Where(artist => artist.AdminId == null).AsNoTracking().ToListAsync();
    }

    public async Task<bool> ApproveArtist(int artistId, int adminId)
    {
        // Check artist existence
        var artist = await _dbContext.Artists.FirstOrDefaultAsync(a => a.Id == artistId && a.AdminId == null);
        if (artist == null)
            return false;
        
        // Update admin id, hire date and return states
        artist.AdminId = adminId;
        artist.HireDate = DateTime.UtcNow;
        return await _saveChanges.Save();
    }

    public async Task<bool> RejectArtist(int artistId)
    {
        // Check artist existence
        var artist = await _dbContext.Artists.FirstOrDefaultAsync(a => a.Id == artistId && a.AdminId == null);
        if (artist == null)
            return false;
        
        // Delete artist
        _dbContext.Remove(artist);
        return await _saveChanges.Save();
    }
}