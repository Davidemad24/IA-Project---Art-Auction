namespace ArtAuction.Application.DTOs.Auth;

public class AuthResponseDto
{
    // ضيفي السطر ده هنا 👇
    public int UserId { get; set; }

    public string Token { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Role { get; set; } = null!;
    public bool RequiresApproval { get; set; } = false;
    public DateTime ExpiresAt { get; set; }
}