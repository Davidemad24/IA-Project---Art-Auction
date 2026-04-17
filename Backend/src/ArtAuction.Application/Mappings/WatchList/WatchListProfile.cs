using ArtAuction.Application.DTOs.ArtworkPost;
using ArtAuction.Application.Entities;
using AutoMapper;

namespace ArtAuction.Application.Mappings;

public class WatchListProfile : Profile
{
    public WatchListProfile()
    {
        // This links WatchList to the ArtworkPost mapping logic
        CreateMap<WatchList, ArtworkPostDto>()
            .IncludeMembers(watchList => watchList.ArtworkPost);
    }
}