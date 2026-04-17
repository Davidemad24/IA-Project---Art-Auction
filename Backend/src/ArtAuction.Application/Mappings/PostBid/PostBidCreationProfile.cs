using ArtAuction.Application.DTOs.ArtworkPost;
using AutoMapper;

namespace ArtAuction.Application.Mappings.PostBid;

public class PostBidCreationProfile : Profile
{
    public PostBidCreationProfile()
    {
        CreateMap<PostBidCreationDto, Entities.PostBid>()
            .ForMember(postBid => postBid.ArtworkPost, 
                opt => opt.Ignore())
            .ForMember(postBid => postBid.Buyer, 
                opt => opt.Ignore());
    }
}