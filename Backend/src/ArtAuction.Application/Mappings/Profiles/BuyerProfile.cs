using ArtAuction.Application.DTOs.Profiles;
using ArtAuction.Application.Entities;
using AutoMapper;

namespace ArtAuction.Application.Mappings.Profiles;

public class BuyerProfile : Profile
{
    public BuyerProfile()
    {
        CreateMap<Buyer, BuyerProfileDto>()
            // Map name
            .ForMember(buyerProfileDto => buyerProfileDto.Name,
                opt => opt.MapFrom(buyer => buyer.Name));
    }
}