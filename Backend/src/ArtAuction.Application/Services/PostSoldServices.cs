using ArtAuction.Application.DTOs.PostSold;
using ArtAuction.Application.Entities;
using ArtAuction.Application.Interfaces.Repositories;
using ArtAuction.Application.Interfaces.Services;
using AutoMapper;

namespace ArtAuction.Application.Services;

public class PostSoldServices : IPostSoldServices
{
    // Attributes
    private readonly IMapper _mapper;
    private readonly IPostSoldRepo _postSoldRepo;
    
    // Constructor
    public PostSoldServices(IMapper mapper, IPostSoldRepo postSoldRepo)
    {
        this._mapper = mapper;
        this._postSoldRepo = postSoldRepo;
    }
    
    // Methods
    public async Task<ICollection<PostSoldDto>> GetAllPostSolds()
    {
        // Get post sold data
        var postSolds = await _postSoldRepo.GetAllPostSolds();
        
        // Return mapped DTO
        return _mapper.Map<ICollection<PostSoldDto>>(postSolds);
    }

    public async Task<UnpaidPostSoldDto> GetUnpaidPostSoldForBuyer(int buyerId)
    {
        // Get unpaid post for buyer
        var unpaidPost = await _postSoldRepo.GetUnpaidPostSoldByBuyer(buyerId);
        
        // Return mapped DTO
        return _mapper.Map<UnpaidPostSoldDto>(unpaidPost);
    }

    public async Task<bool> MarkAsPaid(PostSoldPaidDto postSoldPaidDto)
    {
        // Mark post sold record as paid
        return await _postSoldRepo.MarkAsPaid(postSoldPaidDto.ArtworkPostId, postSoldPaidDto.BuyerId);
    }
}