namespace ArtAuction.Application.Entities;

public class Category
{
    // Attributes
    public int Id { get; set; }
    public string Name { get; set; }
    
    // Relationships
    public ICollection<ArtworkPost> ArtworkPosts { get; set; }
}