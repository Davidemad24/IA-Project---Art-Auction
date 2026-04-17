using ArtAuction.Application.DTOs.Tag;
using Microsoft.AspNetCore.Http;

namespace ArtAuction.Application.DTOs.ArtworkPost;

public class ArtworkPostDto
{
    // Attributes
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; } 
    public decimal InitialPrice { get; set; }
    public decimal BuyNewPrice { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Image { get; set; }
    public string ArtistName { get; set; }
    public string CategoryName { get; set; }
    public string[] Tags { get; set; }
}