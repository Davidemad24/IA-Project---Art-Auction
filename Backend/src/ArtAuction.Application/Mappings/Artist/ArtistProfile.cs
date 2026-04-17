using ArtAuction.Application.DTOs.Artist;
using AutoMapper;

namespace ArtAuction.Application.Mappings.Artist;

public class ArtistProfile : Profile
{
    public ArtistProfile()
    {
        CreateMap<Entities.Artist, ArtistDto>()
            // Map name
            .ForMember(artistDto => artistDto.Name, 
                opt => opt.MapFrom(artist => artist.Name));
    }
}