using ArtAuction.Application.DTOs.PostSold;
using ArtAuction.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ArtAuction.API.Controllers;

[ApiController]
[Route("[controller]")]
public class PostSoldController : ControllerBase
{
    // Attributes
    private readonly IPostSoldServices _postSoldServices;
    
    // Constructor
    public PostSoldController(IPostSoldServices postSoldServices)
    {
        this._postSoldServices = postSoldServices;
    }
    
    // Get post sold API
    [HttpGet("GetAllPostSold")]
    public async Task<IActionResult> GetAllPostSold()
    {
        // Return all post sold
        return Ok(await _postSoldServices.GetAllPostSolds());
    }
    
    // Get Unpaid posts API
    [HttpGet("GetUnpaidPostForBuyer")]
    public async Task<IActionResult> GetUnpaidPostForBuyer(int buyerId)
    {
        // Return unpaid post for buyer
        return Ok(await _postSoldServices.GetUnpaidPostSoldForBuyer(buyerId));
    }
    
    // Create post sold API
    [HttpPost("CreatePostSold")]
    public async Task<IActionResult> CreatePostSold([FromBody] PostSoldCreationDto postSoldCreationDto)
    {
        // Check model state
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        // Check creation stats
        var result = await _postSoldServices.CreatePostSold(postSoldCreationDto);
        if (!result)
            return StatusCode(500, "An error occurred while recording the sale.");
        
        // Return success stats
        return Ok("Sale recorded successfully.");
    }
    
    // Update buyer API
    [HttpPatch("UpdateBuyer")]
    public async Task<IActionResult> UpdateBostBuyer([FromBody] PostSoldCreationDto postSoldUpdatingDto)
    {
        // Check model state
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        // Check updating stats
        var result = await _postSoldServices.UpdatePostBuyer(postSoldUpdatingDto);
        if (!result)
            return NotFound("Could not update the buyer. The sale record may not exist.");
        
        // Return success stats
        return Ok("Successfully updated the buyer.");
    }
    
    // Mark as paid API
    [HttpPatch("MarkAsPaid")]
    public async Task<IActionResult> MarkAsPaid([FromBody] PostSoldPaidDto postSoldPaidDto)
    {
        // Check model state
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        // Check stats
        var result = await _postSoldServices.MarkAsPaid(postSoldPaidDto);
        if (!result)
            return BadRequest("Failed to mark the post as paid. Verify the Post and Buyer IDs.");
        
        // Return success stats
        return Ok("Successfully marked as paid.");
    }
}