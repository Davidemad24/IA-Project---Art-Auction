using ArtAuction.Application.DTOs.PostSold;

namespace ArtAuction.Application.Interfaces.Services;

public interface IPostSoldServices
{
    Task<ICollection<PostSoldDto>> GetAllPostSolds();
    Task<UnpaidPostSoldDto> GetUnpaidPostSoldForBuyer(int buyerId);
    Task<ICollection<BuyerPostSoldDto>> GetBuyerPostSolds(int buyerId);
    Task<bool> MarkAsPaid(PostSoldPaidDto postSoldPaidDto);
}