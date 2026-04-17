using ArtAuction.Application.Entities;

namespace ArtAuction.Application.Interfaces.Repositories;

public interface IArtistRepo
{
    // Query methods
    Task<Artist?> GetArtistById(int artistId);
    Task<List<Artist>> GetUnaprovedArtists();
    
    // Manipulation methods
    Task<bool> ApproveArtist(int artistId, int adminId);
    Task<bool> RejectArtist(int artistId);
}