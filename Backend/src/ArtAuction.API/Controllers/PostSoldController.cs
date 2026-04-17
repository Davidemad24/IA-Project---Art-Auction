using ArtAuction.Application.DTOs.PostSold;
using ArtAuction.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Admin")]
    [HttpGet("GetAllPostSold")]
    public async Task<IActionResult> GetAllPostSold()
    {
        // Return all post sold
        return Ok(await _postSoldServices.GetAllPostSolds());
    }
    
    // Get Unpaid posts API
    [Authorize(Roles = "Buyer")]
    [HttpGet("GetUnpaidPostForBuyer")]
    public async Task<IActionResult> GetUnpaidPostForBuyer(int buyerId)
    {
        // Return unpaid post for buyer
        return Ok(await _postSoldServices.GetUnpaidPostSoldForBuyer(buyerId));
    }
    
    // Mark as paid API
    [Authorize(Roles = "Buyer")]
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