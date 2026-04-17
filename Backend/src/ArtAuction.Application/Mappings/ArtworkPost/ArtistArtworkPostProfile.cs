using ArtAuction.Application.DTOs.ArtworkPost;
using ArtAuction.Application.Entities;
using AutoMapper;

namespace ArtAuction.Application.Mappings;

public class ArtistArtworkPostProfile : Profile
{
    public ArtistArtworkPostProfile()
    {
        CreateMap<ArtworkPost, ArtistArtworkPostDto>()
            // Map image
            .ForMember(artistArtworkPostDto => artistArtworkPostDto.Image,
                opt
                    => opt.MapFrom(artworkPost =>
                        artworkPost.Image != null ? Convert.ToBase64String(artworkPost.Image) : null))

            // Map Category Name from the navigation property
            .ForMember(artistArtworkPostDto => artistArtworkPostDto.CategoryName,
                opt =>
                    opt.MapFrom(artworkPost => artworkPost.Category != null ? artworkPost.Category.Name : string.Empty))

            // Transform the join entity collection into a string array
            .ForMember(artistArtworkPostDto => artistArtworkPostDto.Tags,
                opt =>
                    opt.MapFrom(artworkPost => artworkPost.PostTags.Select(pt => pt.Tag.Name).ToArray()));
    }
}