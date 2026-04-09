using ArtAuction.Application.DTOs.ArtworkPost;

namespace ArtAuction.Application.Interfaces.Services;

public interface IArtworkPostServices
{
    Task<ICollection<ArtworkPostDto>> GetAllArtworkPosts();
    Task<ICollection<ArtistArtworkPostDto>> GetAllArtistArtworkPosts(int artistId);
    Task<ArtworkPostDetailedDto> GetArtworkPostDetails(int artworkPostId);
    Task<ICollection<ArtworkPostDto>> GetUnapprovedArtworkPosts();
    
    Task<bool> CreateArtworkPost(ArtworkPostCreationDto artworkPostCreationDto);
    Task<bool> UpdateArtworkPost(ArtworkPostUpdatingDto artworkPostUpdatingDto);
    Task<bool> DeleteArtworkPost(int artworkPostId);
    Task<bool> UpdateEndDate(int artworkPostId, DateTime endDate);
    Task<bool> MarkAsApproved(int artworkPostId, int adminId);
}