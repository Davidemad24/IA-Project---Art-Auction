using ArtAuction.Application.DTOs.Profiles;
using ArtAuction.Application.Entities;
using AutoMapper;

namespace ArtAuction.Application.Mappings.Profiles;

public class AdminProfile : Profile
{
     public AdminProfile()
     {
          CreateMap<Admin, AdminProfileDto>()
               // Map user name
               .ForMember(adminProfileDto => adminProfileDto.Name,
                    opt => 
                         opt.MapFrom(admin => admin.Name));
     }
}