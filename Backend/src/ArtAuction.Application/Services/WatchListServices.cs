using ArtAuction.Application.DTOs.ArtworkPost;
using ArtAuction.Application.DTOs.WatchList;
using ArtAuction.Application.Entities;
using ArtAuction.Application.Interfaces.Repositories;
using ArtAuction.Application.Interfaces.Services;
using AutoMapper;

namespace ArtAuction.Application.Services;

public class WatchListServices : IWatchListServices
{
    // Attributes
    private readonly IMapper _mapper;
    private readonly IWatchListRepo _watchListRepo;

    // Constructor
    public WatchListServices(IMapper mapper, IWatchListRepo watchListRepo)
    {
        this._mapper = mapper;
        this._watchListRepo = watchListRepo;
    }

    // Methods
    public async Task<ICollection<ArtworkPostDto>> GetWatchListForBuyer(int buyerId)
    {
        // Get buyer watch list
        var watchLists = await _watchListRepo.GetWatchListForBuyer(buyerId);
        
        // Return mapped DTO
        return _mapper.Map<ICollection<ArtworkPostDto>>(watchLists);
    }

    public async Task<bool> CreateWatchList(WatchListCreationDto watchListCreationDto)
    {
        // Map DTO to entity
        var watchList = _mapper.Map<WatchList>(watchListCreationDto);
        
        // Add relation and return stats
        return await _watchListRepo.CreateWatchList(watchList);
    }

    public async Task<bool> DeleteWatchList(int buyerId, int artworkPostId)
    {
        // Delete relation
        return await _watchListRepo.DeleteWatchList(buyerId, artworkPostId);
    }
}
