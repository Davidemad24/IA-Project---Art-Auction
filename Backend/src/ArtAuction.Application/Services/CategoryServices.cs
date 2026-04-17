using ArtAuction.Application.DTOs.Category;
using ArtAuction.Application.Entities;
using ArtAuction.Application.Interfaces.Repositories;
using ArtAuction.Application.Interfaces.Services;
using AutoMapper;

namespace ArtAuction.Application.Services;

public class CategoryServices : ICategoryServices
{
    // Attributes
    private readonly IMapper _mapper;
    private readonly ICategoryRepo _categoryRepo;

    // Constructor
    public CategoryServices(IMapper mapper, ICategoryRepo categoryRepo)
    {
        _mapper = mapper;
        _categoryRepo = categoryRepo;
    }

    // Methods
    public async Task<ICollection<CategoryDto>> GetAllCategories()
    {
        // Get all categories
        var categories = await _categoryRepo.GetAllCategories();
        
        // Return mapped DTO
        return _mapper.Map<ICollection<CategoryDto>>(categories);
    }
}
