using ArtAuction.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ArtAuction.API.Controllers;

[ApiController]
[Route("[controller]")]
public class TagController : ControllerBase
{
    // Attributes
    private readonly ITagServices _tagServices;
    
    // Constructor
    public TagController(ITagServices tagServices)
    {
        this._tagServices = tagServices;
    }

    // Get all tags API
    [HttpGet("GetAllTags")]
    public async Task<IActionResult> GetAllTags()
    {
        // Return all tags
        return Ok(await _tagServices.GetAllTags());
    }
}
