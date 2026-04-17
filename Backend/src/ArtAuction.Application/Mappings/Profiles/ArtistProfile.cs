using ArtAuction.Application.DTOs.Artist;
using ArtAuction.Application.DTOs.Profiles;
using ArtAuction.Application.Entities;
using AutoMapper;

namespace ArtAuction.Application.Mappings.Profiles;

public class ArtistProfile : Profile
{
    public ArtistProfile()
    {
        CreateMap<Entities.Artist, ArtistProfileDto>()
            // Map user name
            .ForMember(artistProfileDto => artistProfileDto.Name, 
                opt => opt.MapFrom(artist => artist.Name));
    }
}