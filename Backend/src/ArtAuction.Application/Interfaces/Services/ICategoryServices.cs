using ArtAuction.Application.DTOs.Category;

namespace ArtAuction.Application.Interfaces.Services;

public interface ICategoryServices
{
    Task<ICollection<CategoryDto>> GetAllCategories();
}
