using ArtAuction.Application.DTOs.PostSold;
using AutoMapper;

namespace ArtAuction.Application.Mappings.PostSold;

public class PostSoldProfile : Profile
{
    public PostSoldProfile()
    {
        CreateMap<Entities.PostSold, PostSoldDto>()
            // Get the Name from the Buyer navigation property
            .ForMember(postSoldDto => postSoldDto.BuyerName,
                opt => 
                    opt.MapFrom(postSold => postSold.Buyer.UserName))

            // Nested Mapping: AutoMapper will automatically look for a Map 
            .ForMember(postSoldDto => postSoldDto.ArtworkPost,
                opt => 
                    opt.MapFrom(postSold => postSold.ArtworkPost));
    }
}