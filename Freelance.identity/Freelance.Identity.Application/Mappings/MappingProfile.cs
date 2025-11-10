using AutoMapper;
using Freelance.Identity.Application.Dtos;
using Freelance.Identity.Domain.Entities;

namespace Freelance.Identity.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Account, AccountDto>()
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new AddressDto
            {
                Street = src.Address.Street,
                City = src.Address.City,
                State = src.Address.State,
                ZipCode = src.Address.ZipCode,
                Country = src.Address.Country,
                StreetNumber = src.Address.StreetNumber
            }))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.Value))
            .ReverseMap();
    }
}