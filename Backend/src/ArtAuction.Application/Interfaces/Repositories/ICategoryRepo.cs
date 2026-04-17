using ArtAuction.Application.Entities;

namespace ArtAuction.Application.Interfaces.Repositories;

public interface ICategoryRepo
{
    // Query methods
    Task<ICollection<Category>> GetAllCategories();
}
