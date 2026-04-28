namespace ArtAuction.Application.DTOs.Authentication;

public class AuthenticationDto
{
    // User data
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public int? adminId { get; set; }
    
    // Authentication data
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public DateTime ExpiresOn { get; set; }
    public string Message { get; set; }
    public bool IsAuthenticated { get; set; }
}