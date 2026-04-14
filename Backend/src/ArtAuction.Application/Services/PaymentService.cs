using ArtAuction.Application.DTOs;
using ArtAuction.Application.DTOs.Payment;
using ArtAuction.Application.Entities;
using ArtAuction.Application.Interfaces;
using ArtAuction.Application.Interfaces.Repositories;
using ArtAuction.Application.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ArtAuction.Application.Services;

public class PaymentService : IPaymentService
{
    private readonly IPostSoldRepository _postSoldRepo;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly INotificationService _notification;
    private readonly IAppDbContext _context;

    public PaymentService(
        IPostSoldRepository postSoldRepo,
        UserManager<ApplicationUser> userManager,
        INotificationService notification,
        IAppDbContext context)
    {
        _postSoldRepo = postSoldRepo;
        _userManager = userManager;
        _notification = notification;
        _context = context;
    }

    // يتم استدعاؤها تلقائياً بعد انتهاء الـ Auction (بواسطة زميلك)
    // أو يديرها الأدمن يدوياً
    public async Task<Result<WinnerDto>> DetermineAndSaveWinnerAsync(int artworkPostId)
    {
        // Check if winner already determined
        var existing = await _postSoldRepo.GetByArtworkPostIdAsync(artworkPostId);
        if (existing != null)
            return Result<WinnerDto>.Success(await MapToWinnerDto(existing));

        // Get the highest bid from PostBids (زميلك مسؤول عن PostBid)
        var highestBid = await _context.PostBids
            .Where(pb => pb.ArtworkPostId == artworkPostId)
            .OrderByDescending(pb => pb.BuyerPrice)
            .FirstOrDefaultAsync();

        if (highestBid == null)
            return Result<WinnerDto>.Failure("No bids found for this artwork.");

        var postSold = new PostSold
        {
            BuyerId = highestBid.BuyerId,
            ArtworkPostId = artworkPostId,
            FinalPrice = highestBid.BuyerPrice,
            IsPaid = false
        };

        await _postSoldRepo.AddAsync(postSold);
        await _postSoldRepo.SaveChangesAsync();

        var saved = await _postSoldRepo.GetByArtworkPostIdAsync(artworkPostId);
        if (saved == null)
            return Result<WinnerDto>.Failure("Failed to save sale record.");

        var winnerDto = await MapToWinnerDto(saved);

        await _notification.SendWinnerNotificationAsync(
            saved.Buyer!.UserId,
            artworkPostId,
            saved.FinalPrice,
            winnerDto.ArtworkTitle);

        return Result<WinnerDto>.Success(winnerDto);
    }

    public async Task<Result<bool>> MarkAsPaidAsync(int buyerUserId, int artworkPostId)
    {
        var buyer = await _context.Buyers.FirstOrDefaultAsync(b => b.UserId == buyerUserId);
        if (buyer == null) return Result<bool>.Failure("Buyer profile not found.");

        var postSold = await _postSoldRepo.GetByArtworkPostIdAsync(artworkPostId);
        if (postSold == null) return Result<bool>.Failure("No sale record found.");
        if (postSold.BuyerId != buyer.Id) return Result<bool>.Failure("Unauthorized.");
        if (postSold.IsPaid) return Result<bool>.Failure("Already paid.");

        postSold.IsPaid = true;
        await _postSoldRepo.UpdateAsync(postSold);
        await _postSoldRepo.SaveChangesAsync();

        return Result<bool>.Success(true);
    }

    public async Task<Result<IEnumerable<WinnerDto>>> GetBuyerPurchasesAsync(int buyerUserId)
    {
        var buyer = await _context.Buyers.FirstOrDefaultAsync(b => b.UserId == buyerUserId);
        if (buyer == null) return Result<IEnumerable<WinnerDto>>.Failure("Buyer profile not found.");

        var sales = await _postSoldRepo.GetByBuyerIdAsync(buyer.Id);
        var dtos = new List<WinnerDto>();
        foreach (var s in sales)
            dtos.Add(await MapToWinnerDto(s));
        return Result<IEnumerable<WinnerDto>>.Success(dtos);
    }

    private async Task<WinnerDto> MapToWinnerDto(PostSold ps)
    {
        // Load relations if not loaded
        if (ps.Buyer == null)
            await _context.Entry(ps).Reference(x => x.Buyer).LoadAsync();
        if (ps.ArtworkPost == null)
            await _context.Entry(ps).Reference(x => x.ArtworkPost).LoadAsync();

        var user = await _userManager.FindByIdAsync(ps.Buyer!.UserId.ToString());
        return new WinnerDto
        {
            BuyerId = ps.BuyerId,
            BuyerFullName = user?.FullName ?? "Unknown",
            BuyerEmail = user?.Email ?? "Unknown",
            FinalPrice = ps.FinalPrice,
            ArtworkPostId = ps.ArtworkPostId,
            ArtworkTitle = ps.ArtworkPost?.Title ?? "Unknown",
            IsPaid = ps.IsPaid
        };
    }
}
