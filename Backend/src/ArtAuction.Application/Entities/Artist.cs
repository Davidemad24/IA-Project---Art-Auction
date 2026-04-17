using ArtAuction.Application.Entities;

namespace ArtAuction.Application.Entities;

public class Artist : ApplicationUser
{
    // Attributes
    public string City { get; set; } 
    public string Country { get; set; }
    public DateTime HireDate { get; set; }
    public int? AdminId { get; set; }
    
    // Relationships
    public Admin Admin { get; set; }
    public ICollection<ArtworkPost> ArtworkPosts { get; set; }
}