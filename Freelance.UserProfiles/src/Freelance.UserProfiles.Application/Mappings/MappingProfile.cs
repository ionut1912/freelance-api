using AutoMapper;
using Freelance.UserProfiles.Domain.Entities;
using Freelance.UserProfiles.Domain.ValueObjects;
using Freelancer.UserProfiles.Application.Dtos;

namespace Freelancer.UserProfiles.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ClientProfile, ClientProfileDto>()
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new AddressDto
            {
                Street = src.Address.Street,
                City = src.Address.City,
                State = src.Address.State,
                ZipCode = src.Address.ZipCode,
                Country = src.Address.Country,
                StreetNumber = src.Address.StreetNumber
            }))
            .ReverseMap();

        CreateMap<Address, AddressDto>().ReverseMap();

        CreateMap<FreelancerProfile, FreelancerProfileDto>()
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Rate.Amount))
            .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Rate.Currency))
            .ForMember(dest => dest.ForeignLanguages,
                opt => opt.MapFrom(src => src.ForeignLanguages.Select(l => l.Language)))
            .ForMember(dest => dest.ProgrammingLanguages,
                opt => opt.MapFrom(src => src.Skills.Select(s => s.ProgrammingLanguage)))
            .ForMember(dest => dest.Areas, opt => opt.MapFrom(src => src.Skills.Select(s => s.Area)))
            .ReverseMap();

        CreateMap<FreelancerProfileDto, FreelancerProfile>()
            .ConstructUsing(dto => FreelancerProfile.Create(
                dto.AccountId,
                dto.Address.Street,
                dto.Address.City,
                dto.Address.State,
                dto.Address.ZipCode,
                dto.Address.Country,
                dto.Address.StreetNumber,
                dto.Bio,
                dto.Image,
                dto.Experience,
                dto.Amount,
                dto.Currency,
                dto.Rating,
                dto.PortfolioUrl
            ))
            .AfterMap((dto, entity) =>
            {
                // 🔹 Add languages
                if (dto.ForeignLanguages != null && dto.ForeignLanguages.Any())
                {
                    var languages = dto.ForeignLanguages
                        .Select(lang => FreelancerForeignLanguage.Create(lang))
                        .ToList();

                    entity.AddLanguages(languages);
                }

                // 🔹 Add skills (pairing ProgrammingLanguages & Areas)
                if (dto.ProgrammingLanguages != null && dto.Areas != null)
                {
                    var count = Math.Min(dto.ProgrammingLanguages.Count, dto.Areas.Count);
                    var skills = new List<Skill>();

                    for (var i = 0; i < count; i++) skills.Add(Skill.Create(dto.ProgrammingLanguages[i], dto.Areas[i]));

                    entity.AddSkills(skills);
                }
            });
    }
}