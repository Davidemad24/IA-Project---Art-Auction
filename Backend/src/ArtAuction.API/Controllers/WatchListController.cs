using ArtAuction.Application.DTOs.WatchList;
using ArtAuction.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArtAuction.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = "Buyer")]
public class WatchListController : ControllerBase
{
    // Attributes
    private readonly IWatchListServices _watchListServices;

    // Constructor
    public WatchListController(IWatchListServices watchListServices)
    {
        this._watchListServices = watchListServices;
    }

    // Get buyer watch list
    [HttpGet("GetWatchListForBuyer")]
    public async Task<IActionResult> GetWatchListForBuyer(int buyerId)
    {
        // Return watch list
        return Ok(await _watchListServices.GetWatchListForBuyer(buyerId));
    }

    // Create watch relation API
    [HttpPost("CreateWatchList")]
    public async Task<IActionResult> CreateWatchList([FromBody] WatchListCreationDto watchListCreationDto)
    {
        // Check model state
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Check stats
        var result = await _watchListServices.CreateWatchList(watchListCreationDto);
        if (!result)
            return BadRequest("Could not create watchlist relation. Check IDs or duplicate relation.");

        // Return success stats
        return Ok("Watchlist relation created successfully.");
    }

    // Delete watch relation API
    [HttpDelete("DeleteWatchList")]
    public async Task<IActionResult> DeleteWatchList(int buyerId, int artworkPostId)
    {
        // Check stats
        var result = await _watchListServices.DeleteWatchList(buyerId, artworkPostId);
        if (!result)
            return NotFound("Watchlist relation was not found.");

        // Return success stats
        return Ok("Watchlist relation deleted successfully.");
    }
}
