using ArtAuction.Application.DTOs.ArtworkPost;
using ArtAuction.Application.Entities;
using AutoMapper;

namespace ArtAuction.Application.Mappings;

public class ArtworkPostProfile : Profile
{
    public ArtworkPostProfile()
    {
        CreateMap<ArtworkPost, ArtworkPostDto>()
            // Map Artist Name from the Artist navigation property
            .ForMember(artworkPostDto => artworkPostDto.ArtistName, 
                opt => 
                    opt.MapFrom(src => src.Artist.UserName))
            
            // Map Category Name from the Category navigation property
            .ForMember(artworkPostDto => artworkPostDto.CategoryName, 
                opt => 
                    opt.MapFrom(artworkPost => artworkPost.Category != null ? artworkPost.Category.Name : string.Empty))
            
            // Flatten the PostTags collection into a string array of Tag names
            .ForMember(artworkPostDto => artworkPostDto.Tags, 
                opt =>
                    opt.MapFrom(artworkPost => artworkPost.PostTags.Select(pt => pt.Tag.Name).ToArray()));
    }
}