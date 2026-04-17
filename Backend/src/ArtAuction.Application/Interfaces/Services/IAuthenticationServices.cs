using ArtAuction.Application.DTOs.Authentication;

namespace ArtAuction.Application.Interfaces.Services;

public interface IAuthenticationServices
{
    Task<AuthenticationDto> RegisterBuyer(RegisterBuyerDto dto);
    Task<AuthenticationDto> RegisterArtist(RegisterArtistDto dto);
    Task<AuthenticationDto> Login(LoginDto dto);
    Task<AuthenticationDto> RefreshToken(string token);
    Task<bool> Logout(int userId);
}