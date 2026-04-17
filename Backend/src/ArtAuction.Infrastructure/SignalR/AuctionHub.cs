using Microsoft.AspNetCore.SignalR;

namespace ArtAuction.Infrastructure.SignalR;

public class AuctionHub : Hub
{
    public async Task JoinAuction(string auctionId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, auctionId);
    }
}