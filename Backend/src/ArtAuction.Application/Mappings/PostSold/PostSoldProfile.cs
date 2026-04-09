using ArtAuction.Application.DTOs.PostSold;
using AutoMapper;

namespace ArtAuction.Application.Mappings.PostSold;

public class PostSoldProfile : Profile
{
    public PostSoldProfile()
    {
        CreateMap<Entities.PostSold, PostSoldDto>()
            // 1. Flattening: Get the Name from the Buyer navigation property
            .ForMember(dest => dest.BuyerName, 
                opt => 
                    opt.MapFrom(src => src.Buyer.UserName)) 

            // 3. Nested Mapping: AutoMapper will automatically look for a Map 
            // between ArtworkPost (Entity) and ArtworkPostDto (DTO)
            .ForMember(dest => dest.ArtworkPost, 
                opt => 
                    opt.MapFrom(src => src.ArtworkPost));
    }
}