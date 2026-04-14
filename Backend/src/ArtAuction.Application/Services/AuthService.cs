using ArtAuction.Application.DTOs;
using ArtAuction.Application.DTOs.Auth;
using ArtAuction.Application.Entities;
using ArtAuction.Application.Interfaces;
using ArtAuction.Application.Interfaces.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ArtAuction.Application.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole<int>> _roleManager;
    private readonly IAppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole<int>> roleManager,
        IAppDbContext context,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
        _configuration = configuration;
    }

    public async Task<Result<AuthResponseDto>> RegisterBuyerAsync(RegisterBuyerDto dto)
    {
        var existingUser = await _userManager.FindByEmailAsync(dto.Email);
        if (existingUser != null)
            return Result<AuthResponseDto>.Failure("Email already exists");

        if (dto.Password != dto.ConfirmPassword)
            return Result<AuthResponseDto>.Failure("Passwords do not match");

        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            FullName = dto.FullName,
            CreatedAt = DateTime.UtcNow
        };

        var create = await _userManager.CreateAsync(user, dto.Password);
        if (!create.Succeeded)
            return Result<AuthResponseDto>.Failure(string.Join(", ", create.Errors.Select(e => e.Description)));

        const string role = "Buyer";
        if (!await _roleManager.RoleExistsAsync(role))
            await _roleManager.CreateAsync(new IdentityRole<int>(role));

        await _userManager.AddToRoleAsync(user, role);

        var buyer = new Buyer
        {
            UserId = user.Id,
            City = dto.City,
            Country = dto.Country,
            PhoneNumber = dto.PhoneNumber,
            Address = dto.Address
        };
        _context.Buyers.Add(buyer);
        await _context.SaveChangesAsync();

        var token = GenerateJwtToken(user);
        return Result<AuthResponseDto>.Success(new AuthResponseDto
        {
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email!,
            Token = token,
            Role = role
        });
    }

    public async Task<Result<AuthResponseDto>> RegisterArtistAsync(RegisterArtistDto dto)
    {
        var existingUser = await _userManager.FindByEmailAsync(dto.Email);
        if (existingUser != null)
            return Result<AuthResponseDto>.Failure("Email already exists");

        if (dto.Password != dto.ConfirmPassword)
            return Result<AuthResponseDto>.Failure("Passwords do not match");

        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            FullName = dto.FullName,
            CreatedAt = DateTime.UtcNow
        };

        var create = await _userManager.CreateAsync(user, dto.Password);
        if (!create.Succeeded)
            return Result<AuthResponseDto>.Failure(string.Join(", ", create.Errors.Select(e => e.Description)));

        const string role = "Artist";
        if (!await _roleManager.RoleExistsAsync(role))
            await _roleManager.CreateAsync(new IdentityRole<int>(role));

        await _userManager.AddToRoleAsync(user, role);

        var artist = new Artist
        {
            UserId = user.Id,
            City = dto.City,
            Country = dto.Country,
            PhoneNumber = dto.PhoneNumber,
            HireDate = DateTime.UtcNow,
            // التعديلات :
            IsApproved = false,             
            ApprovedByAdminId = null,       
            AdminId = 1                     
        };

        _context.Artists.Add(artist);
        await _context.SaveChangesAsync();

        var token = GenerateJwtToken(user);
        return Result<AuthResponseDto>.Success(new AuthResponseDto
        {
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email!,
            Token = token,
            Role = role,
            RequiresApproval = true
        });
    }

    public async Task<Result<AuthResponseDto>> LoginAsync(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            return Result<AuthResponseDto>.Failure("Invalid email or password");

        var roles = await _userManager.GetRolesAsync(user);

        // التحقق من حالة الموافقة لو كان يوزر فنان
        if (roles.Contains("Artist"))
        {
            var artist = await _context.Artists.FirstOrDefaultAsync(a => a.UserId == user.Id);
            if (artist == null || !artist.IsApproved)
            {
                return Result<AuthResponseDto>.Failure("Your Artist account is pending admin approval. You cannot login yet.");
            }
        }

        user.LastLoginAt = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        var token = GenerateJwtToken(user);

        return Result<AuthResponseDto>.Success(new AuthResponseDto
        {
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email!,
            Token = token,
            Role = roles.FirstOrDefault() ?? "Buyer"
        });
    }

    public async Task LogoutAsync(int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user != null)
        {
            user.LastLoginAt = null;
            await _userManager.UpdateAsync(user);
        }
    }

    private string GenerateJwtToken(ApplicationUser user)
    {
        // استخدام Task.Run أو GetAwaiter عشان نجيب الـ Roles جوه ميثود Sync
        var roles = _userManager.GetRolesAsync(user).GetAwaiter().GetResult();

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.FullName),
            new(ClaimTypes.Email, user.Email!)
        };

        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}