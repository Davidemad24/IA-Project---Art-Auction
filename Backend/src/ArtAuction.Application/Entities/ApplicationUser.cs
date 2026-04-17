using Microsoft.AspNetCore.Identity;

namespace ArtAuction.Application.Entities;

public class ApplicationUser : IdentityUser<int>
{
    // Attributes
    public string Name { get; set; }
    
    // Relationships
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}