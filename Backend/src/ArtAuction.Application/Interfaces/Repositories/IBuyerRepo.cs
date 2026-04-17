using ArtAuction.Application.Entities;

namespace ArtAuction.Application.Interfaces.Repositories;

public interface IBuyerRepo
{
    // Query methods
    Task<Buyer?> GetBuyerById(int buyerId);
}