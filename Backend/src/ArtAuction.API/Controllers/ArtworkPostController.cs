using ArtAuction.Application.DTOs.ArtworkPost;
using ArtAuction.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArtAuction.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ArtworkPostController : ControllerBase
{
    // Attributes
    private readonly IArtworkPostServices _artworkPostServices;
    
    // Constructor
    public ArtworkPostController(IArtworkPostServices artworkPostServices)
    {
        this._artworkPostServices = artworkPostServices;
    }

    // Get all post API
    [HttpGet("GetAllArtworkPosts")]
    public async Task<IActionResult> GetAllArtworkPosts()
    {
        // Return status with all posts
        return Ok(await _artworkPostServices.GetAllArtworkPosts());
    }

    // Get artist posts API
    [Authorize(Roles = "Artist, Admin")]
    [HttpGet("GetAllArtistPosts")]
    public async Task<IActionResult> GetAllArtistPosts(int artistId)
    {
        // Return status with all artist post
        return Ok(await _artworkPostServices.GetAllArtistArtworkPosts(artistId));
    }
    
    // Get post details API
    [HttpGet("GetPostWithDetails")]
    public async Task<IActionResult> GetPostWithDetails(int artworkPostId)
    {
        // Return status and post with all details
        return Ok(await _artworkPostServices.GetArtworkPostDetails(artworkPostId));
    }
    
    // Get unapproved API
    [Authorize(Roles = "Admin")]
    [HttpGet("GetUnapprovedArtworkPosts")]
    public async Task<IActionResult> GetUnapprovedArtworkPosts()
    {
        // Return status with unapproved posts
        return Ok(await _artworkPostServices.GetUnapprovedArtworkPosts());
    }

    // Create artwork post API
    [Authorize(Roles = "Artist")]
    [HttpPost("CreateArtworkPost")]
    public async Task<IActionResult> CreateArtworkPost([FromForm] ArtworkPostCreationDto artworkPostCreationDto)
    {
        // Check api state
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        // Check stats
        var result = await _artworkPostServices.CreateArtworkPost(artworkPostCreationDto);
        if (!result)
            return StatusCode(500, "An error occurred while creating the artwork post.");
        
        // Return success stats
        return Ok("Artwork post created successfully.");
    }
    
    // Update artwork post API
    [Authorize(Roles = "Artist")]
    [HttpPut("UpdateArtworkPost")]
    public async Task<IActionResult> UpdateArtworkPost([FromForm] ArtworkPostUpdatingDto artworkPostUpdatingDto)
    {
        // Check api state
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        // Check stats
        var result = await _artworkPostServices.UpdateArtworkPost(artworkPostUpdatingDto);
        if (!result)
            return NotFound($"Could not update post. ID {artworkPostUpdatingDto.Id} may not exist.");
        
        // Return success stats
        return Ok("Artwork post updated successfully.");
    }
    
    // Delete artwork post API
    [Authorize(Roles = "Artist")]
    [HttpDelete("DeleteArtworkPost")]
    public async Task<IActionResult> DeleteArtworkPost(int artworkPostId)
    {
        // Check api state
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        // Check stats
        var result = await _artworkPostServices.DeleteArtworkPost(artworkPostId);
        if (!result)
            return NotFound($"Could not update post. ID {artworkPostId} may not exist.");
        
        // Return success stats
        return Ok("Artwork post deleted successfully.");
    }
    
    // Change end date API
    [Authorize(Roles = "Artist")]
    [HttpPatch("ChangeEndDate")]
    public async Task<IActionResult> ChangeEndDate(int artworkPostId, [FromQuery] DateTime endDate)
    {
        // Check api state
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        // Check stats
        var result = await _artworkPostServices.UpdateEndDate(artworkPostId, endDate);
        if (!result)
            return BadRequest("Failed to update the end date.");
        
        // Return success stats
        return Ok("End date updated successfully.");
    }
    
    // Approve artwork post API
    [Authorize(Roles = "Admin")]
    [HttpPatch("ApproveArtworkPost")]
    public async Task<IActionResult> ApproveArtworkPost(int artworkPostId, [FromQuery] int adminId)
    {
        // Check api state
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        // Check stats
        var result = await _artworkPostServices.MarkAsApproved(artworkPostId, adminId);
        if (!result)
            return BadRequest("Approval failed. Please check the post and admin IDs.");
        
        // Return success stats
        return Ok("Artwork post approved successfully.");
    }
}