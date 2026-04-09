using ArtAuction.Application.DTOs.ArtworkPost;
using ArtAuction.Application.Interfaces.Repositories;
using ArtAuction.Application.Interfaces.Services;
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
    
    // Create post bid API
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
    
    // Update post bid API
    [HttpPut("UpdatePostBid")]
    public async Task<IActionResult> UpdatePostBid([FromBody] PostBidCreationDto postBidUpdatingDto)
    {
        // Check model state
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        // Check stats
        var result = await _postBidServices.UpdatePostBid(postBidUpdatingDto);
        if (!result)
            return NotFound("Could not update bid. The bid might not exist or the auction may have ended.");
        
        // Return success stats
        return Ok("Bid updated successfully. Wait for final results to appear.");
    }
    
    // Delete post bid API
    [HttpDelete("DeletePostBid")]
    public async Task<IActionResult> DeletePostBid([FromQuery] int postBidId, [FromQuery] int buyerId)
    {
        // Check model state
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        // Check stats
        var result = await _postBidServices.DeletePostBid(postBidId, buyerId);
        if (!result)
            return NotFound("Bid not found or could not be deleted.");
        
        // Return success stats
        return Ok("Bid deleted successfully.");
    }
}