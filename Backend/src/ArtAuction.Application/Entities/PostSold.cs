namespace ArtAuction.Application.Entities;

public class PostSold
{
    // Attributes
    public int BuyerId { get; set; }
    public int ArtworkPostId { get; set; } 
    public decimal FinalPrice { get; set; }
    public bool IsPaid { get; set; }

    // Relationships
    public Buyer Buyer { get; set; }
    public ArtworkPost ArtworkPost { get; set; }
}