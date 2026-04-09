using ArtAuction.Application.DTOs.ArtworkPost;
using ArtAuction.Application.Entities;
using AutoMapper;

namespace ArtAuction.Application.Mappings;

public class ArtworkPostDetailedProfile : Profile
{
    public ArtworkPostDetailedProfile()
    {
        // 2. Map the main ArtworkPost entity
        CreateMap<ArtworkPost, ArtworkPostDetailedDto>()
            // Map image
            .ForMember(dest => dest.Image, opt 
                => opt.MapFrom(src => src.Image != null ? Convert.ToBase64String(src.Image) : null))
            
            // Map Category Name
            .ForMember(dest => dest.CategoryName,
                opt =>
                    opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty))

            // Map Tags from the Join Table
            .ForMember(dest => dest.Tags,
                opt => 
                    opt.MapFrom(src => src.PostTags != null
                    ? src.PostTags.Select(pt => pt.Tag.Name).ToArray()
                    : new string[0]))

            // Map the Bids collection (AutoMapper maps PostBid -> PostBidDto automatically)
            .ForMember(dest => dest.PostBid,
                opt => 
                    opt.MapFrom(src => src.PostBids))

            // ArtistName must be handled manually since the navigation to User is missing
            .ForMember(artworkPostDto => artworkPostDto.ArtistName,
                opt =>
                    opt.MapFrom(src => src.Artist.UserName));
    }
}