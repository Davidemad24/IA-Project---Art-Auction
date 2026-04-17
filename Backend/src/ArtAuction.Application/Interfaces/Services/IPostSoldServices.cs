using ArtAuction.Application.DTOs.PostSold;
using ArtAuction.Application.Entities;

namespace ArtAuction.Application.Interfaces.Services;

public interface IPostSoldServices
{
    Task<ICollection<PostSoldDto>> GetAllPostSolds();
    Task<UnpaidPostSoldDto> GetUnpaidPostSoldForBuyer(int buyerId);
    Task<bool> MarkAsPaid(PostSoldPaidDto postSoldPaidDto);
}