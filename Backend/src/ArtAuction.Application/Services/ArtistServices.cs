using ArtAuction.Application.DTOs.Artist;
using ArtAuction.Application.Interfaces.Services;
using ArtAuction.Application.Interfaces.Repositories;
using AutoMapper;

namespace ArtAuction.Application.Services;

public class ArtistServices : IArtistServices
{
    // Attributes
    private readonly IArtistRepo _artistRepo;
    private readonly IMapper _mapper;
    
    // Constructor
    public ArtistServices(IArtistRepo artistRepo, IMapper mapper)
    {
        _artistRepo = artistRepo;
        _mapper = mapper;
    }
    
    // Methods
    public async Task<ICollection<ArtistDto>> GetUnapprovedArtists()
    {
        // Get unapproved artist
        var artists = await _artistRepo.GetUnaprovedArtists();
        
        // Return mapped DTO
        return _mapper.Map<ICollection<ArtistDto>>(artists);
    }

    public async Task<ICollection<ArtistDto>> GetApprovedArtists()
    {
        // Get approved artist
        var artists = await _artistRepo.GetApprovedArtists();
        
        // Return mapped DTO
        return _mapper.Map<ICollection<ArtistDto>>(artists);
    }

    public async Task<bool> ApproveArtist(int artistId, int adminId)
    {
        // Approve artist and return states
        return await _artistRepo.ApproveArtist(artistId, adminId);
    }

    public async Task<bool> RejectArtist(int artistId)
    {
        // Reject artist and return states
        return await _artistRepo.RejectArtist(artistId);
    }
}