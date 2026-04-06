namespace ArtAuction.Application.Entities;

public class PostBid
{
    // Attributes
    public int BuyerId { get; set; }
    public int ArtworkPostId { get; set; }
    public decimal BuyerPrice { get; set; } 

    // Relationships
    public Buyer Buyer { get; set; }
    public ArtworkPost ArtworkPost { get; set; }
}