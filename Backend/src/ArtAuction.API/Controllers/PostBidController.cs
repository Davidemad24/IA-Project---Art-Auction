using ArtAuction.Application.DTOs.ArtworkPost;
using ArtAuction.Application.Interfaces.Repositories;
using ArtAuction.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArtAuction.API.Controllers;

[ApiController]
[Route("[controller]")]
public class PostBidController : ControllerBase
{
    // Attributes
    private readonly IPostBidServices _postBidServices;
    
    // Constructor
    public PostBidController(IPostBidServices postBidServices)
    {
        this._postBidServices = postBidServices;
    }
    
    // Get post bids API
    [HttpGet("GetAllPostBids")]
    public async Task<IActionResult> GetAllPostBids([FromQuery] int artworkPostId)
    {
        // Return post bids
        return Ok(await _postBidServices.GetAllPostBids(artworkPostId));
    }
    
    // Get buyer post bids
    [Authorize(Roles = "Buyer")]
    [HttpGet("GetBuyerPostBids")]
    public async Task<IActionResult> GetBuyerPostBids([FromQuery] int buyerId)
    {
        // Return post bids
        return Ok(await _postBidServices.GetAllBuyerBids(buyerId));
    }
    
    // Create post bid API
    [Authorize(Roles = "Buyer")]
    [HttpPost("CreatePostBid")]
    public async Task<IActionResult> CreatePostBid([FromBody] PostBidCreationDto postBidCreationDto)
    {
        // Check model state
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        // Check stats
        var result = await _postBidServices.CreatePostBid(postBidCreationDto);
        if (!result)
            return BadRequest("Could not place bid. Ensure the post is still active.");
        
        // Return success stats
        return Ok("Bid placed successfully. Wait for final results to appear.");
    }
    
    // Delete post bid API
    [Authorize(Roles = "Buyer")]
    [HttpDelete("DeletePostBid")]
    public async Task<IActionResult> DeletePostBid([FromQuery] int artworkPostId, [FromQuery] int buyerId)
    {
        // Check model state
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        // Check stats
        var result = await _postBidServices.DeletePostBid(artworkPostId, buyerId);
        if (!result)
            return NotFound("Bid not found or could not be deleted.");
        
        // Return success stats
        return Ok("Bid deleted successfully.");
    }
}