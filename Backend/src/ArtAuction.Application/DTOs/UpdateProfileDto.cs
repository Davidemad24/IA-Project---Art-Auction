namespace ArtAuction.Application.DTOs.Profile;

public class UpdateProfileDto
{
    public string? FullName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    // Buyer only
    public string? Address { get; set; }
}