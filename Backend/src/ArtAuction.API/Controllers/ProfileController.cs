using ArtAuction.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArtAuction.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ProfileController : ControllerBase
{
    // Attributes
    private readonly IProfileServices _profileServices;
    
    // Constructor
    public ProfileController(IProfileServices profileServices)
    {
        _profileServices = profileServices;
    }
    
    // Admin profile API
    [Authorize(Roles = "Admin")]
    [HttpGet("AdminProfile")]
    public async Task<IActionResult> GetAdminProfile(int adminId)
    {
        return Ok(await _profileServices.GetAdminProfile(adminId));
    }
    
    // Artist profile API
    [Authorize(Roles = "Artist")]
    [HttpGet("ArtistProfile")]
    public async Task<IActionResult> GetArtistProfile(int artistId)
    {
        return Ok(await _profileServices.GetArtistProfile(artistId));
    }

    // Buyer profile API
    [Authorize(Roles = "Buyer")]
    [HttpGet("BuyerProfile")]
    public async Task<IActionResult> GetBuyerProfile(int buyerId)
    {
        return Ok(await _profileServices.GetBuyerProfile(buyerId));
    }
}