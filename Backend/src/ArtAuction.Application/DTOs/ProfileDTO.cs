namespace ArtAuction.Application.DTOs.Profile;

public class ProfileDto
{
    public int UserId { get; set; }
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? ProfilePictureUrl { get; set; }
    public string Role { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
    // Shared (Artist & Buyer)
    public string? PhoneNumber { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    // Buyer only
    public string? Address { get; set; }
    // Artist only
    public DateTime? HireDate { get; set; }
    public bool IsApproved { get; set; }
}