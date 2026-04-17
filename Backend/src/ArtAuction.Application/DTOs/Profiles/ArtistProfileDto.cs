namespace ArtAuction.Application.DTOs.Profiles;

public class ArtistProfileDto
{
    public int  Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string City { get; set; } 
    public string Country { get; set; } 
    public string PhoneNumber { get; set; }
    public DateTime HireDate { get; set; }
}