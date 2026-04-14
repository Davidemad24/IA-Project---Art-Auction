using ArtAuction.Application.DTOs.Profile;
using ArtAuction.Application.Entities;
using ArtAuction.Application.Interfaces;
using ArtAuction.Application.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ArtAuction.Application.Services;

public class ProfileService : IProfileService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IAppDbContext _context;

    public ProfileService(UserManager<ApplicationUser> userManager, IAppDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<ArtAuction.Application.DTOs.Result<ProfileDto>> GetProfileAsync(int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null) return ArtAuction.Application.DTOs.Result<ProfileDto>.Failure("User not found.");

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? "Unknown";

        var dto = new ProfileDto
        {
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email!,
            ProfilePictureUrl = user.ProfilePictureUrl,
            Role = role,
            CreatedAt = user.CreatedAt,
            LastLoginAt = user.LastLoginAt
        };

        if (role == "Artist")
        {
            var artist = await _context.Artists.FirstOrDefaultAsync(a => a.UserId == userId);
            if (artist != null)
            {
                dto.PhoneNumber = artist.PhoneNumber;
                dto.City = artist.City;
                dto.Country = artist.Country;
                dto.HireDate = artist.HireDate;
                dto.IsApproved = artist.AdminId != 0;
            }
        }
        else if (role == "Buyer")
        {
            var buyer = await _context.Buyers.FirstOrDefaultAsync(b => b.UserId == userId);
            if (buyer != null)
            {
                dto.PhoneNumber = buyer.PhoneNumber;
                dto.City = buyer.City;
                dto.Country = buyer.Country;
                dto.Address = buyer.Address;
            }
        }

        return ArtAuction.Application.DTOs.Result<ProfileDto>.Success(dto);
    }

    public async Task<ArtAuction.Application.DTOs.Result<ProfileDto>> UpdateProfileAsync(int userId, UpdateProfileDto dto)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null) return ArtAuction.Application.DTOs.Result<ProfileDto>.Failure("User not found.");

        if (!string.IsNullOrWhiteSpace(dto.FullName)) user.FullName = dto.FullName;
        if (dto.ProfilePictureUrl != null) user.ProfilePictureUrl = dto.ProfilePictureUrl;

        await _userManager.UpdateAsync(user);

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault();

        if (role == "Artist")
        {
            var artist = await _context.Artists.FirstOrDefaultAsync(a => a.UserId == userId);
            if (artist != null)
            {
                if (!string.IsNullOrWhiteSpace(dto.PhoneNumber)) artist.PhoneNumber = dto.PhoneNumber;
                if (!string.IsNullOrWhiteSpace(dto.City)) artist.City = dto.City;
                if (!string.IsNullOrWhiteSpace(dto.Country)) artist.Country = dto.Country;
                _context.Artists.Update(artist);
            }
        }
        else if (role == "Buyer")
        {
            var buyer = await _context.Buyers.FirstOrDefaultAsync(b => b.UserId == userId);
            if (buyer != null)
            {
                if (!string.IsNullOrWhiteSpace(dto.PhoneNumber)) buyer.PhoneNumber = dto.PhoneNumber;
                if (!string.IsNullOrWhiteSpace(dto.City)) buyer.City = dto.City;
                if (!string.IsNullOrWhiteSpace(dto.Country)) buyer.Country = dto.Country;
                if (!string.IsNullOrWhiteSpace(dto.Address)) buyer.Address = dto.Address;
                _context.Buyers.Update(buyer);
            }
        }

        await _context.SaveChangesAsync();
        return await GetProfileAsync(userId);
    }
}