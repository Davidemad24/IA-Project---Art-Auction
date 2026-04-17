using ArtAuction.Application.DTOs.ArtworkPost;
using ArtAuction.Application.Entities;
using ArtAuction.Application.Interfaces.Repositories;
using ArtAuction.Application.Interfaces.Services;
using AutoMapper;

namespace ArtAuction.Application.Services;

public class PostBidServices : IPostBidServices
{
    // Attributes
    private readonly IMapper _mapper;
    private readonly IPostBidRepo _postBidRepo;
    
    // Constructor
    public PostBidServices(IMapper mapper, IPostBidRepo postBidRepo)
    {
        this._mapper = mapper;
        this._postBidRepo = postBidRepo;
    }
    
    // Methods
    public async Task<ICollection<BuyerPostBidDto>> GetAllBuyerBids(int buyerId)
    {
        // Get post bids
        var postBids = await _postBidRepo.GetAllBuyerBids(buyerId);
        
        // Return mapped DTO
        return _mapper.Map<ICollection<BuyerPostBidDto>>(postBids);
    }

    public async Task<bool> CreatePostBid(PostBidCreationDto postBidCreationDto)
    {
        // Map DTO to entity
        var postBid = _mapper.Map<PostBid>(postBidCreationDto);
        
        // Return boolean
        return await _postBidRepo.CreatePostBid(postBid);
    }

    public async Task<bool> DeletePostBid(int artworkPostId, int buyerId)
    {
        // Delete entity and return boolean
        return await _postBidRepo.DeletePostBid(artworkPostId, buyerId);
    }
}