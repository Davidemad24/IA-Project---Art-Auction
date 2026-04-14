using ArtAuction.Application.DTOs;
using ArtAuction.Application.DTOs.Artist;
using ArtAuction.Application.Entities;
using ArtAuction.Application.Interfaces.Repositories;
using ArtAuction.Application.Interfaces.Services;
using Microsoft.AspNetCore.Identity;

namespace ArtAuction.Application.Services;

public class ArtistManagementService : IArtistManagementService
{
    private readonly IArtistRepository _artistRepo;
    private readonly IAdminRepository _adminRepo;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly INotificationService _notification;

    public ArtistManagementService(
        IArtistRepository artistRepo,
        IAdminRepository adminRepo,
        UserManager<ApplicationUser> userManager,
        INotificationService notification)
    {
        _artistRepo = artistRepo;
        _adminRepo = adminRepo;
        _userManager = userManager;
        _notification = notification;
    }

    public async Task<Result<IEnumerable<ArtistDto>>> GetPendingArtistsAsync()
    {
        // هيجيب الفنانين اللي IsApproved = false من الـ Repository
        var artists = await _artistRepo.GetPendingArtistsAsync();
        var dtos = new List<ArtistDto>();

        foreach (var a in artists)
        {
            var user = await _userManager.FindByIdAsync(a.UserId.ToString());
            if (user != null) dtos.Add(MapToDto(a, user));
        }
        return Result<IEnumerable<ArtistDto>>.Success(dtos);
    }

    public async Task<Result<IEnumerable<ArtistDto>>> GetAllArtistsAsync()
    {
        var artists = await _artistRepo.GetAllArtistsAsync();
        var dtos = new List<ArtistDto>();
        foreach (var a in artists)
        {
            var user = await _userManager.FindByIdAsync(a.UserId.ToString());
            if (user != null) dtos.Add(MapToDto(a, user));
        }
        return Result<IEnumerable<ArtistDto>>.Success(dtos);
    }

    public async Task<Result<ArtistDto>> GetArtistByIdAsync(int artistId)
    {
        var artist = await _artistRepo.GetByIdAsync(artistId);
        if (artist == null) return Result<ArtistDto>.Failure("Artist not found.");

        var user = await _userManager.FindByIdAsync(artist.UserId.ToString());
        if (user == null) return Result<ArtistDto>.Failure("User not found.");

        return Result<ArtistDto>.Success(MapToDto(artist, user));
    }

    public async Task<Result<bool>> ApproveOrRejectArtistAsync(int adminUserId, ArtistApprovalDto dto)
    {
        var admin = await _adminRepo.GetByUserIdAsync(adminUserId);
        if (admin == null)
            return Result<bool>.Failure("Admin profile not found.");

        var artist = await _artistRepo.GetByIdAsync(dto.ArtistId);
        if (artist == null) return Result<bool>.Failure("Artist not found.");

        if (dto.IsApproved)
        {
            // التعديل هنا: تحديث الحقول الجديدة في الداتابيز
            artist.IsApproved = true;
            artist.ApprovedByAdminId = admin.Id;
            artist.AdminId = admin.Id;
            artist.HireDate = DateTime.UtcNow;

            await _artistRepo.UpdateAsync(artist);
            await _artistRepo.SaveChangesAsync();
            await _notification.SendArtistApprovalNotificationAsync(artist.UserId, true);
        }
        else
        {
            // رفض الفنان: قفل الحساب ومسح البروفايل
            var user = await _userManager.FindByIdAsync(artist.UserId.ToString());
            if (user != null) await _userManager.SetLockoutEnabledAsync(user, true);

            await _artistRepo.DeleteAsync(artist);
            await _artistRepo.SaveChangesAsync();
            await _notification.SendArtistApprovalNotificationAsync(artist.UserId, false);
        }

        return Result<bool>.Success(true);
    }

    // تعديل الـ Mapping عشان يقرأ القيم الصح من الـ Entity
    private static ArtistDto MapToDto(Artist a, ApplicationUser user) => new()
    {
        ArtistId = a.Id,
        UserId = a.UserId,
        FullName = user.FullName,
        Email = user.Email!,
        PhoneNumber = a.PhoneNumber,
        City = a.City,
        Country = a.Country,
        HireDate = a.HireDate,
        // قراءة مباشرة من الأعمدة الجديدة
        IsApproved = a.IsApproved,
        ApprovedByAdminId = a.ApprovedByAdminId
    };
}