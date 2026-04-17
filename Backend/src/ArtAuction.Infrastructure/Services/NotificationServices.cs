using ArtAuction.Application.Interfaces.Services;
using ArtAuction.Infrastructure.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace ArtAuction.Infrastructure.Services;

public class NotificationServices : INotificationServices
{
    // Attributes
    private readonly IHubContext<AuctionHub> _hub;

    public NotificationServices(IHubContext<AuctionHub> hub)
    {
        _hub = hub;
    }

    public async Task SendToUserAsync(string userId, object data)
    {
        await _hub.Clients.User(userId)
            .SendAsync("ReceiveNotification", data);
    }
}