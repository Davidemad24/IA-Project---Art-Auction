namespace ArtAuction.Application.Entities;

public class Tag
{
    // Attributes
    public int Id { get; set; }
    public string Name { get; set; } 
    
    // Relationships
    public ICollection<PostTag> PostTags { get; set; }
}