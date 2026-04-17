using ArtAuction.Application.DTOs.WatchList;
using ArtAuction.Application.Entities;
using AutoMapper;

namespace ArtAuction.Application.Mappings;

public class WatchListCreationProfile : Profile
{
    public WatchListCreationProfile()
    {
        CreateMap<WatchListCreationDto, WatchList>()
            // Ignore navigation properties
            .ForMember(watchList => watchList.Buyer, 
                opt => opt.Ignore())
            .ForMember(watchList => watchList.ArtworkPost, 
                opt => opt.Ignore());
    }
}
