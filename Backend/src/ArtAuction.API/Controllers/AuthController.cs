using ArtAuction.Application.DTOs;
using ArtAuction.Application.DTOs.Auth;
using ArtAuction.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ArtAuction.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;
    public AuthController(IAuthService auth) => _auth = auth;

    /// <summary>Register a new Buyer account</summary>
    [HttpPost("register/buyer")]
    public async Task<IActionResult> RegisterBuyer([FromBody] RegisterBuyerDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _auth.RegisterBuyerAsync(dto);
        if (!result.IsSuccess) return BadRequest(new { result.Error });
        return StatusCode(201, result.Data);
    }

    /// <summary>Register a new Artist account (requires admin approval)</summary>
    [HttpPost("register/artist")]
    public async Task<IActionResult> RegisterArtist([FromBody] RegisterArtistDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _auth.RegisterArtistAsync(dto);
        if (!result.IsSuccess) return BadRequest(new { result.Error });
        return StatusCode(201, new
        {
            Message = "Registration submitted. Awaiting admin approval.",
            result.Data?.FullName,
            result.Data?.Email
        });
    }

    /// <summary>Login for all roles</summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _auth.LoginAsync(dto);
        if (!result.IsSuccess) return Unauthorized(new { result.Error });
        return Ok(result.Data);
    }
}