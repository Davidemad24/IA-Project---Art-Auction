using ArtAuction.Application.DTOs.ArtworkPost;
using ArtAuction.Application.Entities;
using ArtAuction.Application.Interfaces.Repositories;
using ArtAuction.Application.Interfaces.Services;
using AutoMapper;

namespace ArtAuction.Application.Services;

public class ArtworkPostServices : IArtworkPostServices
{
    // Attributes
    private readonly IMapper _mapper;
    private readonly IArtworkPostRepo _artworkPostRepo;
    private readonly IPostBidRepo _postBidRepo;
    //private readonly IArtistRepo _artistRepo;
    
    // Constructor
    public ArtworkPostServices(IMapper mapper, IArtworkPostRepo artworkPostRepo,
        IPostBidRepo postBidRepo/*, IArtistRepo artistRepo*/)
    {
        this._mapper = mapper;
        this._artworkPostRepo = artworkPostRepo;
        this._postBidRepo = postBidRepo;
        //this._artistRepo = artistRepo;
    }
    
    public async Task<ICollection<ArtworkPostDto>> GetAllArtworkPosts()
    {
        // Get artworkPost and Artist name
        var artworkPosts = await _artworkPostRepo.GetAllArtworkPosts();
        
        // Get tags for each post
        foreach (var artworkPost in artworkPosts)
        {
            artworkPost.PostTags = new List<PostTag>(); // Replace with tags repo method
        }
        
        // Return mapped DTO
        return _mapper.Map<ICollection<ArtworkPostDto>>(artworkPosts);
    }

    public async Task<ICollection<ArtistArtworkPostDto>> GetAllArtistArtworkPosts(int artistId)
    {
        // Get artwork posts for artist
        var artworkPosts = await _artworkPostRepo.GetAllArtworkPostsForArtist(artistId);
        
        // Get tags for each post
        foreach (var artworkPost in artworkPosts)
        {
            artworkPost.PostTags = new List<PostTag>(); // Replace with tags repo method
        }
        
        // Return mapped DTO
        return _mapper.Map<ICollection<ArtistArtworkPostDto>>(artworkPosts);
    }

    public async Task<ArtworkPostDetailedDto> GetArtworkPostDetails(int artworkPostId)
    {
        // Get artwork post
        var artworkPost = await _artworkPostRepo.GetArtworkPost(artworkPostId);
        
        // Get tags and bids for each post
        if (artworkPost != null)
        {
            artworkPost.PostTags = new List<PostTag>(); // Replace with tags repo method
            artworkPost.PostBids = await _postBidRepo.GetAllPostBidsForPost(artworkPostId);
        }
        
        // Return mapped DTO
        return _mapper.Map<ArtworkPostDetailedDto>(artworkPost);
    }

    public async Task<ICollection<ArtworkPostDto>> GetUnapprovedArtworkPosts()
    {
        // Get unapproved artworkPost and Artist name
        var unapprovedArtworkPosts = await _artworkPostRepo.GetUnapprovedArtworkPosts();
        
        // Get tags for each post
        foreach (var artworkPost in unapprovedArtworkPosts)
        {
            artworkPost.PostTags = new List<PostTag>(); // Replace with tags repo method
        }
        
        // Return Mapped DTO
        return _mapper.Map<ICollection<ArtworkPostDto>>(unapprovedArtworkPosts);
    }

    public async Task<bool> CreateArtworkPost(ArtworkPostCreationDto artworkPostCreationDto)
    {
        // Map DTO to entity
        var artworkPost = _mapper.Map<ArtworkPost>(artworkPostCreationDto);
        
        // Add artwork post and return stats
        return await _artworkPostRepo.CreateArtworkPost(artworkPost);
    }

    public async Task<bool> UpdateArtworkPost(ArtworkPostUpdatingDto artworkPostUpdatingDto)
    {
        // Map DTO to entity
        var artworkPost = _mapper.Map<ArtworkPost>(artworkPostUpdatingDto);
        
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