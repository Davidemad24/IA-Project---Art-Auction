using ArtAuction.Application.DTOs.ArtworkPost;
using ArtAuction.Application.DTOs.PostSold;

namespace ArtAuction.Application.Interfaces.Services;

public interface IAuctionJobServices
{
    Task EndAuctionJob(int artworkPostId);
}