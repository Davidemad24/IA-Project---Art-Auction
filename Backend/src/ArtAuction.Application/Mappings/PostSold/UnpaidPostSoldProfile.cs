using ArtAuction.Application.DTOs.PostSold;
using AutoMapper;

namespace ArtAuction.Application.Mappings.PostSold;

public class UnpaidPostSoldProfile : Profile
{
    public UnpaidPostSoldProfile()
    {
        CreateMap<Entities.PostSold, UnpaidPostSoldDto>()
            // 2. Map the complex ArtworkPost object
            // This works automatically if CreateMap<ArtworkPost, ArtworkPostDto>() exists
            .ForMember(dest => dest.ArtworkPost, 
                opt => 
                    opt.MapFrom(src => src.ArtworkPost));
    }
}