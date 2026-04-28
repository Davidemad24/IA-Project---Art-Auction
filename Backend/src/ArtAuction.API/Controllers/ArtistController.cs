using ArtAuction.Application.Interfaces.Repositories;
using ArtAuction.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArtAuction.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = "Admin")]
public class ArtistController : ControllerBase
{
    // Attributes
    private readonly IArtistServices _artistServices;

    // Constructor
    public ArtistController(IArtistServices artistServices)
    {
        _artistServices = artistServices;
    }
    
    // Unapproved artist API
    [HttpGet("GetUnapprovedArtists")]
    public async Task<IActionResult> GetUnapprovedArtists()
    {
        return Ok(await _artistServices.GetUnapprovedArtists());
    }
    
    // Get number of artist approved API
    [HttpGet("GetApprovedArtists")]
    public async Task<IActionResult> GetApprovedArtists()
    {
        return Ok(await _artistServices.GetApprovedArtists());
    }
    
    // Approve artist API
    [HttpPatch("ApproveArtist")]
    public async Task<IActionResult> ApproveArtist([FromQuery] int artistId, [FromQuery] int adminId)
    {
        // Check model state
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        // Approve artist and check result
        var result = await _artistServices.ApproveArtist(artistId, adminId);
        if (!result)
            return NotFound($"Artist {artistId} or Admin {adminId} not found or already approved");
        
        // Return success stats
        return Ok("Artist successfully approved");
    }
    
    // Reject artist
    [HttpPatch("RejectArtist")]
    public async Task<IActionResult> RejectArtist([FromQuery] int artistId)
    {
        // Check model state
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        // Approve artist and check result
        var result = await _artistServices.RejectArtist(artistId);
        if (!result)
            return NotFound($"Artist {artistId} not found or already approved");
        
        // Return success stats
        return Ok("Artist successfully rejected");
    }
}