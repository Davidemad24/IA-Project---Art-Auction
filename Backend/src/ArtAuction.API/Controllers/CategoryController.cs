using ArtAuction.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ArtAuction.API.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoryController : ControllerBase
{
    // Attributes
    private readonly ICategoryServices _categoryServices;
    
    // Constructor
    public CategoryController(ICategoryServices categoryServices)
    {
        this._categoryServices = categoryServices;
    }

    // Get all categories API
    [HttpGet("GetAllCategories")]
    public async Task<IActionResult> GetAllCategories()
    {
        // Return stats with categories
        return Ok(await _categoryServices.GetAllCategories());
    }
}
