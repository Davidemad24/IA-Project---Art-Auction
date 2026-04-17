namespace ArtAuction.Application.Entities;

public class RefreshToken
{
    // Attributes
    public int Id { get; set; }
    public string Token { get; set; }
    public DateTime ExpiresOn { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? RevokedOn { get; set; }
    public bool IsActive => RevokedOn == null && DateTime.UtcNow < ExpiresOn;
    public int UserId { get; set; }
    
    // Relationships
    public ApplicationUser User { get; set; }
}