namespace ArtAuction.Application.Entities;

public class Buyer
{
    // Attributes
    public int Id { get; set; }
    public int UserId { get; set; } 
    public string City { get; set; } 
    public string Country { get; set; } 
    public string PhoneNumber { get; set; } 
    public string Address { get; set; } 
    
    // Relationships
    public ICollection<WatchList> WatchLists { get; set; }
    public ICollection<PostBid> PostBids { get; set; }
    public ICollection<PostSold> PostSolds { get; set; }
}