namespace ArtAuction.Application.DTOs;

public class RegisterArtistDto
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string ConfirmPassword { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string City { get; set; } = "";
    public string Country { get; set; } = "";
    public string PhoneNumber { get; set; } = "";
}
