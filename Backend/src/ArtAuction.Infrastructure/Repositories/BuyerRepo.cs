using ArtAuction.Application.Entities;
using ArtAuction.Application.Interfaces.Repositories;
using ArtAuction.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ArtAuction.Infrastructure.Repositories;

public class BuyerRepo : IBuyerRepo
{
    // Attributes
    private readonly AppDbContext _dbContext;
    
    // Constructor
    public BuyerRepo(AppDbContext dbContext, ISaveChanges saveChanges)
    {
        this._dbContext = dbContext;
    }
    
    // Methods
    public async Task<Buyer?> GetBuyerById(int buyerId)
    {
        return await _dbContext.Buyers.AsNoTracking().SingleOrDefaultAsync(buyer => buyer.Id == buyerId);
    }
}