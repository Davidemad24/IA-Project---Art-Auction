namespace ArtAuction.Application.DTOs;

public class RegisterBuyerDto
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string ConfirmPassword { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string City { get; set; } = "Cairo";
    public string Country { get; set; } = "Egypt";
    public string PhoneNumber { get; set; } = "01000000000";
    public string Address { get; set; } = "Default Address";
}
