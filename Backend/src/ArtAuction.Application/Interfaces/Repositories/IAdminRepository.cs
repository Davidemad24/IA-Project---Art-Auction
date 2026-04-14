using ArtAuction.Application.Entities;

namespace ArtAuction.Application.Interfaces.Repositories;

public interface IAdminRepository
{
    Task<Admin?> GetByIdAsync(int id);
    Task<Admin?> GetByUserIdAsync(int userId);
    Task AddAsync(Admin admin);
    Task SaveChangesAsync();
}
