using ArtAuction.Application.DTOs.Artist;

namespace ArtAuction.Application.Interfaces.Services;

public interface IArtistServices
{
    Task<ICollection<ArtistDto>> GetUnapprovedArtists();
    Task<bool> ApproveArtist(int artistId, int adminId);
    Task<bool> RejectArtist(int artistId);
}