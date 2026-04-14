using ArtAuction.Application.Entities;
using ArtAuction.Application.Interfaces.Repositories;
using ArtAuction.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ArtAuction.Infrastructure.Repositories.Implementations;

public class AdminRepository : IAdminRepository
{
    private readonly AppDbContext _context;
    public AdminRepository(AppDbContext context) => _context = context;

    public async Task<Admin?> GetByIdAsync(int id) =>
        await _context.Admins
            .Include(a => a.Artist)
            .Include(a => a.ArtworkPosts)
            .FirstOrDefaultAsync(a => a.Id == id);

    public async Task<Admin?> GetByUserIdAsync(int userId) =>
        await _context.Admins.FirstOrDefaultAsync(a => a.UserId == userId);

    public async Task AddAsync(Admin admin) =>
        await _context.Admins.AddAsync(admin);

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}