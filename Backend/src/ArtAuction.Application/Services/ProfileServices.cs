using ArtAuction.Application.DTOs.Profiles;
using ArtAuction.Application.Interfaces.Repositories;
using ArtAuction.Application.Interfaces.Services;
using AutoMapper;

namespace ArtAuction.Application.Services;

public class ProfileServices : IProfileServices
{
    // Attributes
    private readonly IAdminRepo _adminRepo;
    private readonly IArtistRepo _artistRepo;
    private readonly IBuyerRepo _buyerRepo;
    private readonly IMapper _mapper;
    
    // Constructor
    public ProfileServices(IAdminRepo adminRepo, IArtistRepo artistRepo, IMapper mapper, IBuyerRepo buyerRepo)
    {
        _adminRepo = adminRepo;
        _artistRepo = artistRepo;
        _buyerRepo = buyerRepo;
        _mapper = mapper;
    }
    
    // Methods
    public async Task<AdminProfileDto> GetAdminProfile(int adminId)
    {
        // Get admin profile
        var admin = await _adminRepo.GetAdminById(adminId);
        
        // Return mapped DTO
        return _mapper.Map<AdminProfileDto>(admin);
    }

    public async Task<ArtistProfileDto> GetArtistProfile(int artistId)
    {
        // Get admin profile
        var admin = await _artistRepo.GetArtistById(artistId);
        
        // Return mapped DTO
        return _mapper.Map<ArtistProfileDto>(admin);
    }

    public async Task<BuyerProfileDto> GetBuyerProfile(int buyerId)
    {
        // Get admin profile
        var admin = await _buyerRepo.GetBuyerById(buyerId);
        
        // Return mapped DTO
        return _mapper.Map<BuyerProfileDto>(admin);
    }
}