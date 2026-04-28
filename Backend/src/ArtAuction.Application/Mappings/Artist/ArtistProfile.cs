using ArtAuction.Application.DTOs.Artist;
using AutoMapper;

namespace ArtAuction.Application.Mappings.Artist;

public class ArtistProfile : Profile
{
    public ArtistProfile()
    { 
        CreateMap<Entities.Artist, ArtistDto>()
            // Map name and id
            .ForMember(artistDto => artistDto.Id, 
                opt => opt.MapFrom(artist => artist.Id))
            .ForMember(artistDto => artistDto.Name, 
                opt => opt.MapFrom(artist => artist.Name));
    }
}