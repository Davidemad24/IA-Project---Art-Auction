using ArtAuction.Application.Entities;
using ArtAuction.Application.Interfaces.Repositories;
using ArtAuction.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ArtAuction.Infrastructure.Repositories;

public class CategoryRepo : ICategoryRepo
{
    // Attributes
    private readonly AppDbContext _dbContext;

    // COnstructor
    public CategoryRepo(AppDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    // Method
    public async Task<ICollection<Category>> GetAllCategories()
    {
        return await _dbContext.Categories.AsNoTracking().ToListAsync();
    }
}
