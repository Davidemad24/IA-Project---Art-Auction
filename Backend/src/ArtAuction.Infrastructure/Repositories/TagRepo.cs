using ArtAuction.Application.Entities;
using ArtAuction.Application.Interfaces.Repositories;
using ArtAuction.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ArtAuction.Infrastructure.Repositories;

public class TagRepo : ITagRepo
{
    // Attributes
    private readonly AppDbContext _dbContext;

    // Constructor
    public TagRepo(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // Method
    public async Task<ICollection<Tag>> GetAllTags()
    {
        return await _dbContext.Tags.AsNoTracking().ToListAsync();
    }
}
