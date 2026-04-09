using ArtAuction.Application.Entities;

namespace ArtAuction.Application.Interfaces.Repositories;

public interface IArtworkPostRepo
{
    // Query methods
    Task<ICollection<ArtworkPost>> GetAllArtworkPosts();
    Task<ICollection<ArtworkPost>> GetAllArtworkPostsForArtist(int artistId);
    Task<ArtworkPost?> GetArtworkPost(int artworkPostId);
    Task<ICollection<ArtworkPost>> GetUnapprovedArtworkPosts();

    // Manipulation methods
    Task<bool> CreateArtworkPost(ArtworkPost artworkPost);
    Task<bool> UpdateArtworkPost(ArtworkPost artworkPost);
    Task<bool> DeleteArtworkPost(int artworkPostId);
    Task<bool> UpdateEndDate(int artworkPostId, DateTime endDate);
    Task<bool> MarkAsApproved(int artworkPostId, int adminId);
}