namespace ArtAuction.Application.Entities;

public class Admin : ApplicationUser
{
    // Relationships
    public ICollection<Artist> Artist { get; set; }
    public ICollection<ArtworkPost> ArtworkPosts { get; set; }
}