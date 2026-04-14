namespace ArtAuction.Application.Entities;

public class Artist
{
    // Attributes
    public int Id { get; set; }
    public int UserId { get; set; } 
    public string City { get; set; } 
    public string Country { get; set; } 
    public string PhoneNumber { get; set; }
    public DateTime HireDate { get; set; }
    public int AdminId { get; set; }
    public bool IsApproved { get; set; } = false;
    public int? ApprovedByAdminId { get; set; } // Nullable عشان الفنان الجديد ملوش أدمن لسه

    // Relationships
    public Admin Admin { get; set; }
    public ICollection<ArtworkPost> ArtworkPosts { get; set; }

}