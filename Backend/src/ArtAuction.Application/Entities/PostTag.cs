namespace ArtAuction.Application.Entities;

public class PostTag
{
    public int ArtworkPostId { get; set; }
    public int TagId { get; set; }

    public ArtworkPost ArtworkPost { get; set; }
    public Tag Tag { get; set; }
}