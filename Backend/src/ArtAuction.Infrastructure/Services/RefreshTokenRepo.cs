using ArtAuction.Application.Entities;
using ArtAuction.Application.Interfaces.Repositories;
using ArtAuction.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ArtAuction.Infrastructure.Services;

public class RefreshTokenRepo : IRefreshTokenRepo
{
    // Attributes
    private readonly AppDbContext _dbContext;
    private readonly ISaveChanges _saveChanges;
    
    // Constructor
    public RefreshTokenRepo(AppDbContext dbContext, ISaveChanges saveChanges)
    {
        _dbContext = dbContext;
        _saveChanges = saveChanges;
    }
    
    // Methods
    public async Task<RefreshToken?> GetAsync(string token)
    {
        return await _dbContext.RefreshTokens
            .Include(refreshToken => refreshToken.User)
            .FirstOrDefaultAsync(refreshToken => refreshToken.Token == token);
    }

    public async Task AddAsync(RefreshToken token)
    {
        await _dbContext.RefreshTokens.AddAsync(token);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(RefreshToken token)
    {
        _dbContext.RefreshTokens.Update(token);
        await _saveChanges.Save();
    }
}