using ArtAuction.Application.DTOs.Profiles;

namespace ArtAuction.Application.Interfaces.Services;

public interface IProfileServices
{
    Task<AdminProfileDto> GetAdminProfile(int adminId);
    Task<ArtistProfileDto> GetArtistProfile(int artistId);
    Task<BuyerProfileDto> GetBuyerProfile(int buyerId);
}