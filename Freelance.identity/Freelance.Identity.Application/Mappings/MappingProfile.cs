using AutoMapper;
using Freelance.Identity.Application.Dtos;
using Freelance.Identity.Domain.Entities;

namespace Freelance.Identity.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Account, AccountDto>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.Value))
            .ReverseMap();
    }
}