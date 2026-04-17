using ArtAuction.Application.DTOs.Authentication;
using ArtAuction.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ArtAuction.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    // Attributes
    private readonly IAuthenticationServices _authenticationServices;
    
    // Controller
    public AuthenticationController(IAuthenticationServices authenticationServices)
        =>  _authenticationServices = authenticationServices;
    
    // Buyer registration API
    [HttpPost("BuyerRegistration")]
    public async Task<IActionResult> RegisterBuyer(RegisterBuyerDto registerBuyerDto)
    {
        // Check api state
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        // Register buyer and check results
        var authenticationDto = await _authenticationServices.RegisterBuyer(registerBuyerDto);
        if (!authenticationDto.IsAuthenticated) 
            return BadRequest(authenticationDto.Message);
        
        // Return token
        return Ok( new
        {
            Id = authenticationDto.Id,
            FullName = authenticationDto.Name,
            Email = authenticationDto.Email,
            Role = authenticationDto.Role,
            Token = authenticationDto.Token,
            RefreshToken = authenticationDto.RefreshToken,
            ExpiresOn = authenticationDto.ExpiresOn
        });
    }
    
    // Artist registration API
    [HttpPost("ArtistRegistration")]
    public async Task<IActionResult> RegisterArtist(RegisterArtistDto registerArtistDto)
    {
        // Check api state
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        // Register buyer and check results
        var authenticationDto = await _authenticationServices.RegisterArtist(registerArtistDto);
        if (!authenticationDto.IsAuthenticated) 
            return Unauthorized(authenticationDto.Message);
        
        // Return token
        return Ok( new
        {
            Id = authenticationDto.Id,
            FullName = authenticationDto.Name,
            Email = authenticationDto.Email,
            Role = authenticationDto.Role,
            Token = authenticationDto.Token,
            RefreshToken = authenticationDto.RefreshToken,
            ExpiresOn = authenticationDto.ExpiresOn
        });
    }
    
    // Login API
    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        // Check api state
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        // Login user and check results
        var authenticationDto = await _authenticationServices.Login(loginDto);
        if (!authenticationDto.IsAuthenticated)
            return Unauthorized(authenticationDto.Message);

        // Return token
        return Ok(new
        {
            Id = authenticationDto.Id,
            FullName = authenticationDto.Name,
            Email = authenticationDto.Email,
            Role = authenticationDto.Role,
            Token = authenticationDto.Token,
            RefreshToken = authenticationDto.RefreshToken,
            ExpiresOn = authenticationDto.ExpiresOn
        });
    }
    
    // Refresh token API
    [HttpPost("RefreshToken")]
    public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
    {
        // Get refresh token and check 
        var result = await _authenticationServices.RefreshToken(refreshToken);
        if (!result.IsAuthenticated)
            return Unauthorized(result.Message);

        // Return token
        return Ok(new
        {
            Id = result.Id,
            FullName = result.Name,
            Email = result.Email,
            Role = result.Role,
            Token = result.Token,
            RefreshToken = result.RefreshToken,
            ExpiresOn = result.ExpiresOn
        });
    }
}