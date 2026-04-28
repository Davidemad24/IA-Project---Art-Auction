namespace ArtAuction.Application.Interfaces.Repositories;

public interface IAuthenticationRepo
{
    // Manipulation method
    Task<bool> RevokeRefreshToken(string refreshToken);
}