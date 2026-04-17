using ArtAuction.Application.DTOs.ArtworkPost;
using ArtAuction.Application.Entities;
using AutoMapper;

namespace ArtAuction.Application.Mappings;

public class ArtworkPostDetailedProfile : Profile
{
    public ArtworkPostDetailedProfile()
    {
        CreateMap<ArtworkPost, ArtworkPostDetailedDto>()
            // Map image
            .ForMember(artworkPostDetailedDto => artworkPostDetailedDto.Image, 
                opt 
                    => opt.MapFrom(artworkPost => artworkPost.Image != null ? 
                        Convert.ToBase64String(artworkPost.Image) : null))
            
            // Map Category Name
            .ForMember(artworkPostDetailedDto => artworkPostDetailedDto.CategoryName,
                opt =>
                    opt.MapFrom(artworkPost => artworkPost.Category != null ? artworkPost.Category.Name : string.Empty))

            // Map Tags from the Join Table
            .ForMember(artworkPostDetailedDto => artworkPostDetailedDto.Tags,
                opt => 
                        opt.MapFrom(artworkPost => artworkPost.PostTags != null ? 
                            artworkPost.PostTags.Select(pt => pt.Tag.Name).ToArray() : new string[0]))

            // Map the Bids collection (AutoMapper maps PostBid -> PostBidDto automatically)
            .ForMember(artworkPostDetailedDto => artworkPostDetailedDto.PostBid,
                opt => 
                    opt.MapFrom(artworkPost => artworkPost.PostBids))

            // Map artist name from navigation property
            .ForMember(artworkPostDetailedDto => artworkPostDetailedDto.ArtistName,
                opt =>
                    opt.MapFrom(artworkPost => artworkPost.Artist.UserName));
    }
}