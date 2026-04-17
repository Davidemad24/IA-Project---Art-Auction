using ArtAuction.Application.Entities;
using ArtAuction.Application.Interfaces.Repositories;
using ArtAuction.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ArtAuction.Infrastructure.Repositories;

public class WatchListRepo : IWatchListRepo
{
    private readonly AppDbContext _dbContext;
    private readonly ISaveChanges _saveChanges;

    public WatchListRepo(AppDbContext dbContext, ISaveChanges saveChanges)
    {
        _dbContext = dbContext;
        _saveChanges = saveChanges;
    }

    public async Task<ICollection<WatchList>> GetWatchListForBuyer(int buyerId)
    {
        return await _dbContext.WatchLists
            .Where(watchList => watchList.BuyerId == buyerId)
            .Include(watchList => watchList.ArtworkPost)
            .ThenInclude(artworkPost => artworkPost.Artist)
            .Include(watchList => watchList.ArtworkPost)
            .ThenInclude(artworkPost => artworkPost.Category)
            .Include(watchList => watchList.ArtworkPost)
            .ThenInclude(artworkPost => artworkPost.PostTags)
            .ThenInclude(artworkPost => artworkPost.Tag)
            .AsNoTracking().ToListAsync();
    }

    public async Task<bool> CreateWatchList(WatchList watchList)
    {
        // Check relation duplication
        var isDuplicate = await _dbContext.WatchLists.AnyAsync(wl =>
            wl.BuyerId == watchList.BuyerId && wl.ArtworkPostId == watchList.ArtworkPostId);
        if (isDuplicate)
            return false;

        // Check buyer existence
        var buyerExists = await _dbContext.Buyers
            .AnyAsync(buyer => buyer.Id == watchList.BuyerId);
        if (!buyerExists)
            return false;

        // Check artwork post existence
        var artworkPostExists = await _dbContext.ArtworkPosts
            .AnyAsync(artworkPost => artworkPost.Id == watchList.ArtworkPostId);
        if (!artworkPostExists)
            return false;

        // Add relation
        await _dbContext.WatchLists.AddAsync(watchList);
        return await _saveChanges.Save();
    }

    public async Task<bool> DeleteWatchList(int buyerId, int artworkPostId)
    {
        // Check relation existence
        var watchList = await _dbContext.WatchLists.FirstOrDefaultAsync(wl =>
            wl.BuyerId == buyerId && wl.ArtworkPostId == artworkPostId);
        if (watchList == null)
            return false;

        // Delete relation
        _dbContext.WatchLists.Remove(watchList);
        return await _saveChanges.Save();
    }
}
