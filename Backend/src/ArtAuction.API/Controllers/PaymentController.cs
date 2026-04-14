using System.Security.Claims;
using ArtAuction.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArtAuction.API.Controllers;

[ApiController]
[Route("api/payment")]
[Authorize]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _payment;
    public PaymentController(IPaymentService payment) => _payment = payment;

    private int CurrentUserId =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>Admin or system triggers winner determination after auction ends</summary>
    [HttpPost("determine-winner/{artworkPostId:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DetermineWinner(int artworkPostId)
    {
        var result = await _payment.DetermineAndSaveWinnerAsync(artworkPostId);
        if (!result.IsSuccess) return BadRequest(new { result.Error });
        return Ok(result.Data);
    }

    /// <summary>Winner marks their purchase as paid</summary>
    [HttpPost("mark-paid/{artworkPostId:int}")]
    [Authorize(Roles = "Buyer")]
    public async Task<IActionResult> MarkAsPaid(int artworkPostId)
    {
        // Need to resolve BuyerId from UserId
        var result = await _payment.MarkAsPaidAsync(CurrentUserId, artworkPostId);
        if (!result.IsSuccess) return BadRequest(new { result.Error });
        return Ok(new { Message = "Payment confirmed." });
    }

    /// <summary>Buyer sees their won artworks</summary>
    [HttpGet("my-purchases")]
    [Authorize(Roles = "Buyer")]
    public async Task<IActionResult> GetMyPurchases()
    {
        var result = await _payment.GetBuyerPurchasesAsync(CurrentUserId);
        return Ok(result.Data);
    }

    /// <summary>Get winner info for a specific artwork</summary>
    [HttpGet("winner/{artworkPostId:int}")]
    [Authorize(Roles = "Admin,Artist")]
    public async Task<IActionResult> GetWinner(int artworkPostId)
    {
        var result = await _payment.DetermineAndSaveWinnerAsync(artworkPostId);
        if (!result.IsSuccess) return NotFound(new { result.Error });
        return Ok(result.Data);
    }
}
