using System.Security.Claims;
using ArtAuction.Application.Interfaces.Services;
using ArtAuction.Application.DTOs.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArtAuction.API.Controllers;

[ApiController]
[Route("api/profile")]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly IProfileService _profile;
    public ProfileController(IProfileService profile) => _profile = profile;

    private int CurrentUserId =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> GetMyProfile()
    {
        var result = await _profile.GetProfileAsync(CurrentUserId);
        if (!result.IsSuccess) return NotFound(new { result.Error });
        return Ok(result.Data);
    }

    [HttpGet("{userId:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetProfileById(int userId)
    {
        var result = await _profile.GetProfileAsync(userId);
        if (!result.IsSuccess) return NotFound(new { result.Error });
        return Ok(result.Data);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateProfileDto dto)
    {
        var result = await _profile.UpdateProfileAsync(CurrentUserId, dto);
        if (!result.IsSuccess) return BadRequest(new { result.Error });
        return Ok(result.Data);
    }
}