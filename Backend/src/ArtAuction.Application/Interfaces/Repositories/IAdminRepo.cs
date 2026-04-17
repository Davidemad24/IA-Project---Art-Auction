using ArtAuction.Application.Entities;

namespace ArtAuction.Application.Interfaces.Repositories;

public interface IAdminRepo
{
    Task<Admin?> GetAdminById(int adminId);
}