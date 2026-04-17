using ArtAuction.Application.DTOs.ArtworkPost;
using AutoMapper;

namespace ArtAuction.Application.Mappings.PostBid;

public class BuyerPostBidProfile : Profile
{
    public BuyerPostBidProfile()
    {
        CreateMap<Entities.PostBid, BuyerPostBidDto>()
            // Map artwork title
            .ForMember(buyerPostBidDto => buyerPostBidDto.Title, 
                opt => 
                    opt.MapFrom(postBid => postBid.ArtworkPost.Title));
    }
}