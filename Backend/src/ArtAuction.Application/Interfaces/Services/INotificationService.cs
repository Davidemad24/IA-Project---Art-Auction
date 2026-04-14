namespace ArtAuction.Application.Interfaces.Services;

public interface INotificationService
{
    Task SendWinnerNotificationAsync(int userId, int artworkPostId, decimal finalPrice, string artworkTitle);
    Task SendArtistApprovalNotificationAsync(int userId, bool approved);
}
