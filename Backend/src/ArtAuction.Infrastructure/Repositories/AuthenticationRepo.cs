using ArtAuction.Application.Interfaces.Repositories;
using ArtAuction.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ArtAuction.Infrastructure.Repositories;

public class AuthenticationRepo : IAuthenticationRepo
{
    // Attributes
    private readonly AppDbContext _context;
    private readonly ISaveChanges _saveChanges;
    
    // Constructor
    public AuthenticationRepo(AppDbContext context, ISaveChanges saveChanges)
    {
        _context = context;
        _saveChanges = saveChanges;
    }
    
    // Method
    public async Task<bool> RevokeRefreshToken(string refreshToken)
    {
        // Check refresh token 
        var token = await _context.RefreshTokens.
            SingleOrDefaultAsync(
                refreshTokens => refreshTokens.Token == refreshToken &&
                refreshTokens.RevokedOn == null                    
            );
        if (token == null) return false;

        // Set Revoke date
        token.RevokedOn = DateTime.UtcNow;
        
        // Update and save
        _context.RefreshTokens.Update(token);
        return (await _saveChanges.Save());
    }
}