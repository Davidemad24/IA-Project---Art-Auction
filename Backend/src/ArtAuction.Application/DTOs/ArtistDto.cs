namespace ArtAuction.Application.DTOs.Artist;

public class ArtistDto
{
    public int ArtistId { get; set; }
    public int UserId { get; set; }
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Country { get; set; } = null!;
    public DateTime? HireDate { get; set; }
    public bool IsApproved { get; set; }  // AdminId != 0
    public int? ApprovedByAdminId { get; set; }
}