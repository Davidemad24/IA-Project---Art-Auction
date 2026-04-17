using ArtAuction.Application.DTOs.Authentication;
using AutoMapper;

namespace ArtAuction.Application.Mappings.Authentication;

public class RegisterArtistProfile : Profile
{
    public RegisterArtistProfile()
    {
        CreateMap<RegisterArtistDto, Entities.Artist>()
            // Map name and username
            .ForMember(artist => artist.Name, opt =>
                opt.MapFrom(registerBuyerDto => registerBuyerDto.FullName))
            .ForMember(artist => artist.UserName, opt =>
                opt.MapFrom(registerArtistDto => registerArtistDto.Email))  

            // Ignore navigation properties
            .ForMember(artist => artist.ArtworkPosts, opt =>
                opt.Ignore())
            .ForMember(artist => artist.Admin, opt =>
                opt.Ignore());
    }
}