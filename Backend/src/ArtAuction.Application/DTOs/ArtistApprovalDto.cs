using System.ComponentModel.DataAnnotations;

namespace ArtAuction.Application.DTOs.Artist;

public class ArtistApprovalDto
{
    [Required] public int ArtistId { get; set; }
    [Required] public bool IsApproved { get; set; }
}