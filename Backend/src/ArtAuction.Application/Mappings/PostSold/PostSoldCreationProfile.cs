using ArtAuction.Application.DTOs.PostSold;
using AutoMapper;

namespace ArtAuction.Application.Mappings.PostSold;

public class PostSoldCreationProfile : Profile
{
    public PostSoldCreationProfile()
    {
        CreateMap<PostSoldCreationDto, Entities.PostSold>()
            // Ignore navigation properties so AutoMapper doesn't try to find them in the DTO
            .ForMember(dest => dest.Buyer, 
                opt => opt.Ignore())
            .ForMember(dest => dest.ArtworkPost, 
                opt => opt.Ignore());
    }
}