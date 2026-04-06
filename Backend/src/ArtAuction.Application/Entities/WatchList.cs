namespace ArtAuction.Application.Entities;

public class WatchList
{
    // Attributes
    public int BuyerId { get; set; }
    public int ArtworkPostId { get; set; }

    // Relationships
    public Buyer Buyer { get; set; }
    public ArtworkPost ArtworkPost { get; set; }
}