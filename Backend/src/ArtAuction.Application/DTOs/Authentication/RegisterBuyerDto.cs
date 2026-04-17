namespace ArtAuction.Application.DTOs.Authentication;

public class RegisterBuyerDto
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
}