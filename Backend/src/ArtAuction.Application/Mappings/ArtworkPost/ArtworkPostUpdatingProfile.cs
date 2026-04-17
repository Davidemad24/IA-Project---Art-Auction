using ArtAuction.Application.DTOs.ArtworkPost;
using ArtAuction.Application.Entities;
using AutoMapper;

namespace ArtAuction.Application.Mappings;

public class ArtworkPostUpdatingProfile : Profile
{
    public ArtworkPostUpdatingProfile()
    {
        CreateMap<ArtworkPostUpdatingDto, ArtworkPost>()
            // ADD THIS LINE FIRST
            .ForMember(dest => dest.Id, 
                opt => 
                    opt.MapFrom(src => src.Id))
            
            // Map the Many-to-Many Tags
            .ForMember(artworkPost => artworkPost.PostTags, opt 
                => opt.MapFrom(artworkPostUpdatingDto => 
                    artworkPostUpdatingDto.TagIds.Select(id => new PostTag { TagId = id }).ToList()))
            
            // Map image to array of bytes
            .ForMember(artworkPost => artworkPost.Image,
                opt => opt.Ignore())

            // Ignore ID fields (set it ro 0 but Db will make it different)
            .ForMember(artworkPost => artworkPost.AdminId, 
                opt => opt.Ignore()) 
            
            // Ignore navigation properties
            .ForMember(artworkPost => artworkPost.Admin, 
                opt => opt.Ignore())
            .ForMember(artworkPost => artworkPost.PostBids, 
                opt => opt.Ignore())
            .ForMember(artworkPost => artworkPost.PostSolds, 
                opt => opt.Ignore())
            .ForMember(artworkPost => artworkPost.WatchLists, 
                opt => opt.Ignore())
            .ForMember(artworkPost => artworkPost.Category, 
                opt => opt.Ignore())
            .ForMember(artworkPost => artworkPost.Artist, 
                opt => opt.Ignore());
    }
}