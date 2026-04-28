using Microsoft.AspNetCore.SignalR;

namespace ArtAuction.Infrastructure.SignalR;

public class AuctionHub : Hub
{
    public async Task JoinAuctionRoom(string artworkId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, artworkId);
        // Optional: Notify the group a user joined
        // await Clients.Group(artworkId).SendAsync("UserJoined", Context.ConnectionId);
    }

    // Add the missing LeaveAuctionRoom method
    public async Task LeaveAuctionRoom(string artworkId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, artworkId);
    }
}