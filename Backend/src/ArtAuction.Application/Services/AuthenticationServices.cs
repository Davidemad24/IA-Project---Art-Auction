using System.IdentityModel.Tokens.Jwt;
using ArtAuction.Application.DTOs.Authentication;
using ArtAuction.Application.Entities;
using ArtAuction.Application.Enums;
using ArtAuction.Application.Interfaces.Repositories;
using ArtAuction.Application.Interfaces.Services;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace ArtAuction.Application.Services;

public class AuthenticationServices : IAuthenticationServices
{
    // Attributes
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;
    private readonly IJwtServices _jwtServices;
    private readonly IRefreshTokenRepo _refreshTokenRepo;
    private readonly IArtistRepo _artistRepo;
    private readonly IAuthenticationRepo _authenticationRepo;
    
    // Constructor
    public AuthenticationServices(UserManager<ApplicationUser> userManager, IMapper mapper, 
        IJwtServices jwtServices, IRefreshTokenRepo refreshTokenRepo, IArtistRepo artistRepo,
        IAuthenticationRepo authenticationRepo)
    {
        _userManager = userManager;
        _mapper = mapper;
        _jwtServices = jwtServices;
        _refreshTokenRepo = refreshTokenRepo;
        _artistRepo = artistRepo;
        _authenticationRepo = authenticationRepo;
    }
    
    // Methods
    private async Task<AuthenticationDto> Register<AppUser>
        (string email, string password, Roles role, object dto) where AppUser : ApplicationUser
    {
        // Check email existence
        if (await _userManager.FindByEmailAsync(email) is not null)
            return new AuthenticationDto{Message = "Email is already registered"};
        
        // Mapping DTO to entity
        var user = _mapper.Map<AppUser>(dto);
        
        // Create user and check result
        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            // Return errors
            return new AuthenticationDto { 
                Message = string.Join(", ", result.Errors.Select(e => e.Description)) 
            };
        }
        
        // Add role to user
        await _userManager.AddToRoleAsync(user, role.ToString());
        
        // Get jwt token
        var jwtSecurityToken = await _jwtServices.GenerateJwtToken(user);
        
        // Create refresh token
        var refreshToken = GenerateRefreshToken();
        refreshToken.UserId = user.Id;
        await _refreshTokenRepo.AddAsync(refreshToken);
        
        // Mapping data
        var authenticationDto = _mapper.Map<AuthenticationDto>(user);
        authenticationDto.Role = role.ToString();
        authenticationDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        authenticationDto.ExpiresOn = jwtSecurityToken.ValidTo;
        authenticationDto.IsAuthenticated = true;
        authenticationDto.RefreshToken = refreshToken.Token;
        
        // Return token
        return authenticationDto;
    }

    public async Task<AuthenticationDto> RegisterBuyer(RegisterBuyerDto registerBuyerDto)
        => await Register<Buyer>(registerBuyerDto.Email, registerBuyerDto.Password,
            Roles.Buyer, registerBuyerDto);

    public async Task<AuthenticationDto> RegisterArtist(RegisterArtistDto registerArtistDto) 
        => await Register<Artist>(registerArtistDto.Email, registerArtistDto.Password,
            Roles.Artist, registerArtistDto);

    public async Task<AuthenticationDto> Login(LoginDto loginDto)
    {
        var authenticationDto = new AuthenticationDto();
        
        // Check username existence and password validation
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user is null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
        {
            authenticationDto.Message = "Username or password is not correct";
            return authenticationDto;
        }
        
        // Get JWT token and select roles
        var jwtSecurityToken = await _jwtServices.GenerateJwtToken(user);
        var role = await _userManager.GetRolesAsync(user);
        
        // Create refresh token
        var refreshToken = GenerateRefreshToken();
        refreshToken.UserId = user.Id;
        await _refreshTokenRepo.AddAsync(refreshToken);
        
        // Mapping data
        authenticationDto = _mapper.Map<AuthenticationDto>(user);
        authenticationDto.Role = role.FirstOrDefault();
        authenticationDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        authenticationDto.ExpiresOn = jwtSecurityToken.ValidTo;
        authenticationDto.IsAuthenticated = true;
        authenticationDto.RefreshToken = refreshToken.Token;
        
        // Add admin ID if user artist
        if (authenticationDto.Role == "Artist")
        {
            var artist = await _artistRepo.GetArtistById(user.Id);
            authenticationDto.adminId = artist.AdminId;
        }
        
        // Return token
        return authenticationDto;
    }

    public async Task<AuthenticationDto> RefreshToken(string token)
    {
        // Get refresh token and check existence
        var storedToken = await _refreshTokenRepo.GetAsync(token);
        if (storedToken == null || !storedToken.IsActive)
            return new AuthenticationDto { Message = "Invalid refresh token" };

        // Get user from refresh token
        var user = storedToken.User;

        // revoke old token (rotation)
        storedToken.RevokedOn = DateTime.UtcNow;
        await _refreshTokenRepo.UpdateAsync(storedToken);

        // generate new JWT
        var jwtToken = await _jwtServices.GenerateJwtToken(user);

        // generate new refresh token
        var newRefreshToken = GenerateRefreshToken();
        newRefreshToken.UserId = user.Id;
        await _refreshTokenRepo.AddAsync(newRefreshToken);

        // Get roles
        var roles = await _userManager.GetRolesAsync(user);

        // Mapping data
        var authenticationDto = _mapper.Map<AuthenticationDto>(user);
        authenticationDto.Role = roles.FirstOrDefault();
        authenticationDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        authenticationDto.ExpiresOn = jwtToken.ValidTo;
        authenticationDto.RefreshToken = newRefreshToken.Token;
        authenticationDto.IsAuthenticated = true;
        
        // Add admin ID if user artist
        if (authenticationDto.Role == "Artist")
        {
            var artist = await _artistRepo.GetArtistById(user.Id);
            authenticationDto.adminId = artist.AdminId;
        }

        // Return token
        return authenticationDto;
    }

    private RefreshToken GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);

        return new RefreshToken
        {
            Token = Convert.ToBase64String(randomBytes),
            CreatedOn = DateTime.UtcNow,
            ExpiresOn = DateTime.UtcNow.AddDays(30)
        };
    }
    
    public async Task<bool> Logout(string refreshToken)
    {
        // Check empty string
        if (string.IsNullOrWhiteSpace(refreshToken))
            return false;

        // Revoke refresh token
        return await _authenticationRepo.RevokeRefreshToken(refreshToken);
    }
}