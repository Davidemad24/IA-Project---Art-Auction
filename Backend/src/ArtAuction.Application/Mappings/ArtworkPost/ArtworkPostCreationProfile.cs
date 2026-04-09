using ArtAuction.Application.DTOs.ArtworkPost;
using ArtAuction.Application.Entities;
using AutoMapper;

namespace ArtAuction.Application.Mappings;

public class ArtworkPostCreationProfile : Profile
{
    public ArtworkPostCreationProfile()
    {
        CreateMap<ArtworkPostCreationDto, ArtworkPost>()
            // 1. Map the Many-to-Many Tags
            .ForMember(dest => dest.PostTags, opt 
                => opt.MapFrom(src => 
                src.TagIds.Select(id => new PostTag { TagId = id }).ToList()))

            // 2. Explicitly Null/Ignore fields for later Admin processing
            // These will be null (for objects) or 0 (for IDs) in the DB
            .ForMember(dest => dest.Id, 
                opt => opt.Ignore())
            .ForMember(dest => dest.AdminId, 
                opt => opt.Ignore()) 
            .ForMember(dest => dest.Admin, 
                opt => opt.Ignore())
            
            // 3. Ignore relationship collections (should be empty on creation)
            .ForMember(dest => dest.PostBids, 
                opt => opt.Ignore())
            .ForMember(dest => dest.PostSolds, 
                opt => opt.Ignore())
            .ForMember(dest => dest.WatchLists, 
                opt => opt.Ignore())
            
            // 4. Navigation properties
            .ForMember(dest => dest.Category, 
                opt => opt.Ignore())
            .ForMember(dest => dest.Artist, 
                opt => opt.Ignore());
    }
}