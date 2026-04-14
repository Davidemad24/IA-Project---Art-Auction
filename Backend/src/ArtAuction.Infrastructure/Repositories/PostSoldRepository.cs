using ArtAuction.Application.Entities;
using ArtAuction.Application.Interfaces.Repositories;
using ArtAuction.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ArtAuction.Infrastructure.Repositories.Implementations;

public class PostSoldRepository : IPostSoldRepository
{
    private readonly AppDbContext _context;
    public PostSoldRepository(AppDbContext context) => _context = context;

    public async Task<PostSold?> GetByArtworkPostIdAsync(int artworkPostId) =>
        await _context.PostSolds
            .Include(ps => ps.Buyer)
            .Include(ps => ps.ArtworkPost)
            .FirstOrDefaultAsync(ps => ps.ArtworkPostId == artworkPostId);

    public async Task<IEnumerable<PostSold>> GetByBuyerIdAsync(int buyerId) =>
        await _context.PostSolds
            .Include(ps => ps.ArtworkPost)
            .Where(ps => ps.BuyerId == buyerId)
            .ToListAsync();

    public async Task AddAsync(PostSold postSold) =>
        await _context.PostSolds.AddAsync(postSold);

    public Task UpdateAsync(PostSold postSold)
    {
        _context.PostSolds.Update(postSold);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}