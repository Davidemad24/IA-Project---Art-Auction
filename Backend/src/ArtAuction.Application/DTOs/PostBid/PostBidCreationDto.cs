namespace ArtAuction.Application.DTOs.ArtworkPost;

public class PostBidCreationDto
{
    // Attributes
    public int BuyerId { get; set; }
    public int ArtworkPostId { get; set; }
    public decimal BuyerPrice { get; set; } 
}