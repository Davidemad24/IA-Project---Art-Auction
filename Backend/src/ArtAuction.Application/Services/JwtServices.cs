using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ArtAuction.API.Configurations;
using ArtAuction.Application.Interfaces.Services;
using ArtAuction.Application.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace ArtAuction.Application.Services;

public class JwtServices : IJwtServices
{
    // Attributes
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly JwtConfiguration _jwtConfiguration;
    
    // Constructor
    public JwtServices(UserManager<ApplicationUser> userManager, JwtConfiguration jwtConfiguration)
    {
        _userManager = userManager;
        _jwtConfiguration = jwtConfiguration;
    }
    
    // Method
    public async Task<JwtSecurityToken> GenerateJwtToken(ApplicationUser user)
    {
        // Get claims and roles
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);
        
        // Add roles to claims
        var roleClaims = new List<Claim>();
        foreach (var role in roles)
            roleClaims.Add(new Claim(ClaimTypes.Role,role));

        // Create claims, union it with role claims and user claims
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("uid", user.Id.ToString()),
        }.Union(userClaims).Union(roleClaims);

        // Get key and determined hash algorithm
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.SecretKey));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        // generate jwt token and return it
        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwtConfiguration.Issuer,
            audience: _jwtConfiguration.Audience,
            claims: claims,
            expires: DateTime.Now.AddDays(_jwtConfiguration.ExpiryHours),
            signingCredentials: signingCredentials
        );
        return jwtSecurityToken;
    }
}