namespace ArtAuction.Application.Interfaces.Services;

public interface INotificationServices
{
    Task SendToUserAsync(string userId, object data);
}