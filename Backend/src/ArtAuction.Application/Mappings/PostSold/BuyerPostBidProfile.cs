using ArtAuction.Application.DTOs.ArtworkPost;
using ArtAuction.Application.DTOs.PostSold;
using AutoMapper;

namespace ArtAuction.Application.Mappings.PostSold;

public class BuyerPostBidProfile : Profile
{
    public BuyerPostBidProfile()
    {
        CreateMap<Entities.PostSold, BuyerPostSoldDto>()
            // Map post name
            .ForMember(buyerPostSoldDto => buyerPostSoldDto.Title,
                opt =>
                    opt.MapFrom(postSold => postSold.ArtworkPost.Title));
    }
}