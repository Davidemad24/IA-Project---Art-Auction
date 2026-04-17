using ArtAuction.Application.Entities;
using ArtAuction.Application.Interfaces.Repositories;
using ArtAuction.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ArtAuction.Infrastructure.Repositories;

public class AdminRepo : IAdminRepo
{
    // Attributes
    private readonly AppDbContext _dbContext;
    
    // Constructor
    public AdminRepo(AppDbContext dbContext, ISaveChanges saveChanges)
    {
        this._dbContext = dbContext;
    }
    
    // Methods
    public async Task<Admin?> GetAdminById(int adminId)
    {
        return await _dbContext.Admins.AsNoTracking().FirstOrDefaultAsync(admin => admin.Id == adminId);
    }
}