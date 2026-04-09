using Microsoft.AspNetCore.Http;

namespace ArtAuction.Application.DTOs.ArtworkPost;

public class ArtworkPostCreationDto
{
    // Attributes
    public string Title { get; set; }
    public string Description { get; set; } 
    public decimal InitialPrice { get; set; }
    public decimal BuyNewPrice { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public IFormFile Image { get; set; }
    public int CategoryId { get; set; }
    public int ArtistId { get; set; }
    public int[] TagIds { get; set; }
}