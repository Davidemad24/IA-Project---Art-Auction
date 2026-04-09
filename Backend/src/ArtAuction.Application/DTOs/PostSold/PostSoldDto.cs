using ArtAuction.Application.DTOs.ArtworkPost;

namespace ArtAuction.Application.DTOs.PostSold;

public class PostSoldDto
{
    // Attributes
    public ArtworkPostDto ArtworkPost { get; set; }
    public string BuyerName { get; set; }
    public string FinalPrice { get; set; }
    public bool IsPaid { get; set; }
}