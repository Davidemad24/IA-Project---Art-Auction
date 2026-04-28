using ArtAuction.Application.Entities;

namespace ArtAuction.Application.Interfaces.Repositories;

public interface IArtistRepo
{
    // Query methods
    Task<Artist?> GetArtistById(int artistId);
    Task<ICollection<Artist>> GetApprovedArtists();
    Task<ICollection<Artist>> GetUnaprovedArtists();
    
    // Manipulation methods
    Task<bool> ApproveArtist(int artistId, int adminId);
    Task<bool> RejectArtist(int artistId);
}