using ArtAuction.Application.DTOs.ArtworkPost;
using AutoMapper;

namespace ArtAuction.Application.Mappings.PostBid;

public class PostBidProfile : Profile
{
    public PostBidProfile()
    {
        CreateMap<Entities.PostBid, PostBidDto>()
            // Extract name from instance as dictionary
            .ForMember(postBidDto => postBidDto.BuyerName, opt 
                => opt.MapFrom(postBid => postBid.Buyer != null && postBid.Buyer.UserName != null 
                    ? postBid.Buyer.UserName : "Unknown Buyer"));
    }
}