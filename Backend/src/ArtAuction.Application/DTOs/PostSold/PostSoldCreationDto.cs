namespace ArtAuction.Application.DTOs.PostSold;

public class PostSoldCreationDto
{
    // Attributes
    public int BuyerId { get; set; }
    public int ArtworkPostId { get; set; } 
    public decimal FinalPrice { get; set; }
}