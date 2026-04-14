using ArtAuction.Application.DTOs;
using ArtAuction.Application.DTOs.Payment;

namespace ArtAuction.Application.Interfaces.Services;

public interface IPaymentService
{
    Task<Result<WinnerDto>> DetermineAndSaveWinnerAsync(int artworkPostId);
    Task<Result<bool>> MarkAsPaidAsync(int buyerUserId, int artworkPostId);
    Task<Result<IEnumerable<WinnerDto>>> GetBuyerPurchasesAsync(int buyerUserId);
}
