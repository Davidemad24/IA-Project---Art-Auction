namespace ArtAuction.Application.Entities;

public class Admin
{
    // Attributes
    public int Id { get; set; }
    public Guid UserId { get; set; } 
    
    // Relationships
    public ICollection<Artist> Artist { get; set; }
    public ICollection<ArtworkPost> ArtworkPosts { get; set; }
    
}