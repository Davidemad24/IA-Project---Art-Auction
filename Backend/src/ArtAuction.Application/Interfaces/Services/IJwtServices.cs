using System.IdentityModel.Tokens.Jwt;
using ArtAuction.Application.Entities;

namespace ArtAuction.Application.Interfaces.Services;

public interface IJwtServices
{
    Task<JwtSecurityToken> GenerateJwtToken(ApplicationUser user);
}