using ArtAuction.Application.DTOs.ArtworkPost;
using AutoMapper;

namespace ArtAuction.Application.Mappings.PostBid;

public class PostBidCreationProfile : Profile
{
    public PostBidCreationProfile()
    {
        CreateMap<PostBidCreationDto, Entities.PostBid>();
    }
}