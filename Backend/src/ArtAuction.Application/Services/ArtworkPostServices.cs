using ArtAuction.Application.DTOs.ArtworkPost;
using ArtAuction.Application.Entities;
using ArtAuction.Application.Interfaces.Repositories;
using ArtAuction.Application.Interfaces.Services;
using AutoMapper;
using Hangfire;

namespace ArtAuction.Application.Services;

public class ArtworkPostServices : IArtworkPostServices
{
    // Attributes
    private readonly IMapper _mapper;
    private readonly IArtworkPostRepo _artworkPostRepo;
    
    // Constructor
    public ArtworkPostServices(IMapper mapper, IArtworkPostRepo artworkPostRepo)
    {
        this._mapper = mapper;
        this._artworkPostRepo = artworkPostRepo;
    }
    
    // Methods
    public async Task<ICollection<ArtworkPostDto>> GetAllArtworkPosts()
    {
        // Get artworkPost and Artist name
        var artworkPosts = await _artworkPostRepo.GetAllArtworkPosts();
        
        // Return mapped DTO
        return _mapper.Map<ICollection<ArtworkPostDto>>(artworkPosts);
    }

    public async Task<ICollection<ArtistArtworkPostDto>> GetAllArtistArtworkPosts(int artistId)
    {
        // Get artwork posts for artist
        var artworkPosts = await _artworkPostRepo.GetAllArtworkPostsForArtist(artistId);
        
        // Return mapped DTO
        return _mapper.Map<ICollection<ArtistArtworkPostDto>>(artworkPosts);
    }

    public async Task<ArtworkPostDetailedDto> GetArtworkPostDetails(int artworkPostId)
    {
        // Get artwork post
        var artworkPost = await _artworkPostRepo.GetArtworkPost(artworkPostId);
        
        // Return mapped DTO
        return _mapper.Map<ArtworkPostDetailedDto>(artworkPost);
    }

    public async Task<ICollection<ArtworkPostDto>> GetUnapprovedArtworkPosts()
    {
        // Get unapproved artworkPost and Artist name
        var unapprovedArtworkPosts = await _artworkPostRepo.GetUnapprovedArtworkPosts();
        
        // Return Mapped DTO
        return _mapper.Map<ICollection<ArtworkPostDto>>(unapprovedArtworkPosts);
    }

    public async Task<bool> CreateArtworkPost(ArtworkPostCreationDto artworkPostCreationDto)
    {
        // Map DTO to entity
        var artworkPost = _mapper.Map<ArtworkPost>(artworkPostCreationDto);
        
        // Map Image manually 
        if (artworkPostCreationDto.Image != null)
        {
            using var stream = new MemoryStream();
            await artworkPostCreationDto.Image.CopyToAsync(stream);
            artworkPost.Image = stream.ToArray();
        }
        
        // Add artwork post and check result
        var result = await _artworkPostRepo.CreateArtworkPost(artworkPost);
        if (!result)
            return result;
        
        
        // Add job and return stats
        BackgroundJob.Schedule<IAuctionJobServices>(
            job => job.EndAuctionJob(artworkPost.Id),
            artworkPostCreationDto.EndDate - DateTime.UtcNow
        );
        return result;
    }

    public async Task<bool> UpdateArtworkPost(ArtworkPostUpdatingDto artworkPostUpdatingDto)
    {
        // Map DTO to entity
        var artworkPost = _mapper.Map<ArtworkPost>(artworkPostUpdatingDto);
        
        // Map Image manually 
        if (artworkPostUpdatingDto.Image != null)
        {
            using var stream = new MemoryStream();
            await artworkPostUpdatingDto.Image.CopyToAsync(stream);
            artworkPost.Image = stream.ToArray();
        }
        
        // Update artwork post and return stats
        return await _artworkPostRepo.UpdateArtworkPost(artworkPost);
    }

    public async Task<bool> DeleteArtworkPost(int artworkPostId)
    {
        // Delete post and return stats
        return await _artworkPostRepo.DeleteArtworkPost(artworkPostId);
    }

    public async Task<bool> UpdateEndDate(int artworkPostId, DateTime endDate)
    {
        // Update end date and return stats
        return await _artworkPostRepo.UpdateEndDate(artworkPostId, endDate);
    }

    public async Task<bool> MarkAsApproved(int artworkPostId, int adminId)
    {
        // Assign post to specific admin and return stats
        return await _artworkPostRepo.MarkAsApproved(artworkPostId, adminId);
    }
}