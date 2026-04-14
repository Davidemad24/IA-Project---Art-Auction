using ArtAuction.Application.DTOs;
using ArtAuction.Application.DTOs.Profile;

namespace ArtAuction.Application.Interfaces.Services;

public interface IProfileService
{
    Task<Result<ProfileDto>> GetProfileAsync(int userId);
    Task<Result<ProfileDto>> UpdateProfileAsync(int userId, UpdateProfileDto dto);
}
