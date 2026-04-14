using ArtAuction.Application.DTOs;
using ArtAuction.Application.DTOs.Auth;

namespace ArtAuction.Application.Interfaces.Services;

public interface IAuthService
{
    Task<Result<AuthResponseDto>> RegisterBuyerAsync(RegisterBuyerDto dto);
    Task<Result<AuthResponseDto>> RegisterArtistAsync(RegisterArtistDto dto);
    Task<Result<AuthResponseDto>> LoginAsync(LoginDto dto);
    Task LogoutAsync(int userId);
}
