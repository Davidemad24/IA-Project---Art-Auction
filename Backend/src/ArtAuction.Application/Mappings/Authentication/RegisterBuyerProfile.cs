using ArtAuction.Application.DTOs.Authentication;
using ArtAuction.Application.Entities;
using AutoMapper;

namespace ArtAuction.Application.Mappings.Authentication;

public class RegisterBuyerProfile : Profile
{
    public RegisterBuyerProfile()
    {
        CreateMap<RegisterBuyerDto, Buyer>()
            // Map name and phone number
            .ForMember(buyer => buyer.Name, opt => 
                opt.MapFrom(registerBuyerDto => registerBuyerDto.FullName))
            .ForMember(buyer => buyer.UserName, opt =>
                opt.MapFrom(registerBuyerDto => registerBuyerDto.Email))
            
            
            // Ignore navigation properties
            .ForMember(buyer => buyer.WatchLists, opt => 
                opt.Ignore())
            .ForMember(buyer => buyer.PostBids, opt => 
                opt.Ignore())
            .ForMember(buyer => buyer.PostSolds, opt => 
                opt.Ignore());
    }
}