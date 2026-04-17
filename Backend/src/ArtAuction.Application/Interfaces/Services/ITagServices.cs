using ArtAuction.Application.DTOs.Tag;

namespace ArtAuction.Application.Interfaces.Services;

public interface ITagServices
{
    Task<ICollection<TagDto>> GetAllTags();
}
