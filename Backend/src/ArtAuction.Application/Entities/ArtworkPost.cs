namespace ArtAuction.Application.Entities;

public class ArtworkPost
{
    // Attributes
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; } 
    public decimal InitialPrice { get; set; }
    public decimal BuyNewPrice { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public byte[] Image { get; set; }
    public int CategoryId { get; set; }
    public int ArtistId { get; set; }
    public int AdminId { get; set; }
    
    // Relationships
    public Category Category { get; set; }
    public Artist Artist { get; set; }
    public Admin Admin { get; set; }
    public ICollection<PostTag> PostTags { get; set; }
    public ICollection<PostSold> PostSolds { get; set; }
    public ICollection<PostBid> PostBids { get; set; }
    public ICollection<WatchList> WatchLists { get; set; }
}