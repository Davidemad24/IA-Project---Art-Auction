using ArtAuction.Application.DTOs.ArtworkPost;

namespace ArtAuction.Application.DTOs.PostSold;

public class UnpaidPostSoldDto
{
    // Attributes
    public ArtworkPostDto ArtworkPost { get; set; }
    public string FinalPrice { get; set; }
}