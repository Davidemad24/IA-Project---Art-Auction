using ArtAuction.Application.Entities;
using ArtAuction.Application.Interfaces.Repositories;
using ArtAuction.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ArtAuction.Infrastructure.Repositories.Implementations;

public class ArtistRepository : IArtistRepository
{
    private readonly AppDbContext _context;
    public ArtistRepository(AppDbContext context) => _context = context;

    public async Task<Artist?> GetByIdAsync(int id) =>
        await _context.Artists
            .Include(a => a.Admin)
            .Include(a => a.ArtworkPosts)
            .FirstOrDefaultAsync(a => a.Id == id);

    public async Task<Artist?> GetByUserIdAsync(int userId) =>
        await _context.Artists
            .Include(a => a.Admin)
            .FirstOrDefaultAsync(a => a.UserId == userId);

    // التعديل هنا: الـ Pending هو اللي الـ IsApproved بتاعه بـ False
    public async Task<IEnumerable<Artist>> GetPendingArtistsAsync() =>
        await _context.Artists
            .Include(a => a.Admin)
            .Where(a => a.IsApproved == false)
            .OrderByDescending(a => a.HireDate)
            .ToListAsync();

    public async Task<IEnumerable<Artist>> GetAllArtistsAsync() =>
        await _context.Artists
            .Include(a => a.Admin)
            .ToListAsync();

    public async Task AddAsync(Artist artist) =>
        await _context.Artists.AddAsync(artist);

    public Task UpdateAsync(Artist artist)
    {
        _context.Artists.Update(artist);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Artist artist)
    {
        _context.Artists.Remove(artist);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}