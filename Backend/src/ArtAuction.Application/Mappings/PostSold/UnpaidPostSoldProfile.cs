using ArtAuction.Application.DTOs.PostSold;
using AutoMapper;

namespace ArtAuction.Application.Mappings.PostSold;

public class UnpaidPostSoldProfile : Profile
{
    public UnpaidPostSoldProfile()
    {
        CreateMap<Entities.PostSold, UnpaidPostSoldDto>()
            // Nested Mapping: AutoMapper will automatically look for a Map 
            .ForMember(unpaidPostSoldDto => unpaidPostSoldDto.ArtworkPostTitle, 
                opt => 
                    opt.MapFrom(postSold => postSold.ArtworkPost.Title));
    }
}