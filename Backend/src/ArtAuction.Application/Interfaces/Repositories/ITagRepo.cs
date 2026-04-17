using ArtAuction.Application.Entities;

namespace ArtAuction.Application.Interfaces.Repositories;

public interface ITagRepo
{
    // Query methods
    Task<ICollection<Tag>> GetAllTags();
}
