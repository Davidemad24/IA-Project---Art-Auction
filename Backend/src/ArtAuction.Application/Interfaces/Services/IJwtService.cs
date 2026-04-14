using ArtAuction.Application.Entities;

namespace ArtAuction.Application.Interfaces.Services;

public interface IJwtService
{
    Task<string> GenerateTokenAsync(ApplicationUser user);
}
