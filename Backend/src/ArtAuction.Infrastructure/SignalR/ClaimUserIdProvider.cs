using Microsoft.AspNetCore.SignalR;

namespace ArtAuction.Infrastructure.SignalR;

public class ClaimUserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        return connection.User?.FindFirst("uid")?.Value;
    }
}