using ArtAuction.Application.DTOs.Authentication;
using ArtAuction.Application.Entities;
using AutoMapper;

namespace ArtAuction.Application.Mappings.Authentication;

public class AuthenticaitonProfile : Profile
{
    public AuthenticaitonProfile()
    {
        CreateMap<ApplicationUser, AuthenticationDto>()
            // Mapping name
            .ForMember(dest => dest.Name, 
                opt => 
                    opt.MapFrom(src => src.Name))
            
            // Ignore runtime properties
            .ForMember(dest => dest.Role, 
                opt => opt.Ignore())
            .ForMember(dest => dest.Token, 
                opt => opt.Ignore()) 
            .ForMember(dest => dest.ExpiresOn, 
                opt => opt.Ignore()); // Handled in Service
    }
}