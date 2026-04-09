using ArtAuction.Application.Interfaces.Repositories;
using ArtAuction.Infrastructure.Persistence;

namespace ArtAuction.Infrastructure.Repositories;

public class SaveChanges : ISaveChanges
{
    // Attributes
    private readonly AppDbContext _dbContext;
    
    // Constructor
    public SaveChanges(AppDbContext dbContext)
    {
        this._dbContext = dbContext;
    }
    
    public async Task<bool> Save()
    {
        return await _dbContext.SaveChangesAsync() > 0;
    }
}