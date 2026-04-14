using ArtAuction.Application.DTOs;
using ArtAuction.Application.DTOs.Artist;

namespace ArtAuction.Application.Interfaces.Services;

public interface IArtistManagementService
{
    Task<Result<IEnumerable<ArtistDto>>> GetPendingArtistsAsync();
    Task<Result<IEnumerable<ArtistDto>>> GetAllArtistsAsync();
    Task<Result<ArtistDto>> GetArtistByIdAsync(int artistId);
    Task<Result<bool>> ApproveOrRejectArtistAsync(int adminUserId, ArtistApprovalDto dto);
}
