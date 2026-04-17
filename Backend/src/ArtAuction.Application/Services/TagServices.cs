using ArtAuction.Application.DTOs.Tag;
using ArtAuction.Application.Entities;
using ArtAuction.Application.Interfaces.Repositories;
using ArtAuction.Application.Interfaces.Services;
using AutoMapper;

namespace ArtAuction.Application.Services;

public class TagServices : ITagServices
{
    // Attributes
    private readonly IMapper _mapper;
    private readonly ITagRepo _tagRepo;

    // Constructor
    public TagServices(IMapper mapper, ITagRepo tagRepo)
    {
        _mapper = mapper;
        _tagRepo = tagRepo;
    }

    // Methods
    public async Task<ICollection<TagDto>> GetAllTags()
    {
        // Get all tags
        var tags = await _tagRepo.GetAllTags();
        
        // Return mapped DTO
        return _mapper.Map<ICollection<TagDto>>(tags);
    }
}
