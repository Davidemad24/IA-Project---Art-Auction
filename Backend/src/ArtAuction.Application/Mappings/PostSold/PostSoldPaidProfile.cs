using ArtAuction.Application.DTOs.PostSold;
using AutoMapper;

namespace ArtAuction.Application.Mappings.PostSold;

public class PostSoldPaidProfile : Profile
{
    public PostSoldPaidProfile()
    {
        CreateMap<PostSoldPaidDto, Entities.PostSold>()
            // Ensure IsPaid is always true when using this specific DTO
            .ForMember(dest => dest.IsPaid, opt 
                => opt.MapFrom(src => true))
            
            // Ignore properties that shouldn't be overwritten by defaults
            .ForMember(dest => dest.FinalPrice, opt 
                => opt.Ignore())
            .ForMember(dest => dest.Buyer, opt 
                => opt.Ignore())
            .ForMember(dest => dest.ArtworkPost, opt 
                => opt.Ignore());
    }
}