using ArtAuction.Application.DTOs.ArtworkPost;
using AutoMapper;

namespace ArtAuction.Application.Mappings.PostBid;

public class PostBidProfile : Profile
{
    public PostBidProfile()
    {
        CreateMap<Entities.PostBid, PostBidDto>()
            .ForMember(dest => dest.BuyerName, opt 
                => opt.MapFrom((src, dest, destMember, context) => 
            {
                // Retrieve the dictionary passed from the Service layer
                if (context.Items.TryGetValue("BuyerNames", out var obj) && 
                    obj is Dictionary<int, string> buyerNames)
                {
                    return buyerNames.TryGetValue(src.BuyerId, out var name) ? name : "Unknown Buyer";
                }
                return "Unknown Buyer";
            }));
    }
}