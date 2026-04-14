using ArtAuction.Application.Interfaces.Services;

namespace ArtAuction.Infrastructure.Services;

public class NotificationService : INotificationService
{
    public Task SendWinnerNotificationAsync(int userId, int artworkPostId, decimal finalPrice, string artworkTitle)
    {
        // مؤقتًا (Mock)
        Console.WriteLine($"Winner Notification → User:{userId}, Artwork:{artworkPostId}, Price:{finalPrice}");
        return Task.CompletedTask;
    }

    public Task SendArtistApprovalNotificationAsync(int userId, bool approved)
    {
        Console.WriteLine($"Artist {(approved ? "Approved" : "Rejected")} → User:{userId}");
        return Task.CompletedTask;
    }
}
