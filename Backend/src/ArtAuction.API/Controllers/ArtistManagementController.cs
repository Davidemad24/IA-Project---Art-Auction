using System.Security.Claims;
using ArtAuction.Application.Interfaces.Services;
using ArtAuction.Application.DTOs.Artist;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArtAuction.API.Controllers;

[ApiController]
[Route("api/admin/artists")]
[Authorize(Roles = "Admin")]
public class ArtistManagementController : ControllerBase
{
    private readonly IArtistManagementService _artistService;
    public ArtistManagementController(IArtistManagementService s) => _artistService = s;

    private int CurrentAdminUserId =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet("pending")]
    public async Task<IActionResult> GetPending()
    {
        var result = await _artistService.GetPendingArtistsAsync();
        return Ok(result.Data);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _artistService.GetAllArtistsAsync();
        return Ok(result.Data);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _artistService.GetArtistByIdAsync(id);
        if (!result.IsSuccess) return NotFound(new { result.Error });
        return Ok(result.Data);
    }

    [HttpPost("approve")]
    public async Task<IActionResult> ApproveOrReject([FromBody] ArtistApprovalDto dto)
    {
        
        var result = await _artistService.ApproveOrRejectArtistAsync(
            CurrentAdminUserId, dto);
        if (!result.IsSuccess) return BadRequest(new { result.Error });
        return Ok(new { Message = dto.IsApproved ? "Artist approved." : "Artist rejected." });
    }
}