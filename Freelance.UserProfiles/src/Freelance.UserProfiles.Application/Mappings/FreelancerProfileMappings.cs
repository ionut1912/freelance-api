using Freelance.UserProfiles.Application.Dtos;
using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;
using Freelance.UserProfiles.Application.Requests;
using Freelance.UserProfiles.Domain.Entities;

namespace Freelance.UserProfiles.Application.Mappings;

public static class FreelancerProfileMappings
{
    extension(FreelancerProfile profile)
    {
        public FreelancerProfileDto ToDto() => new(
            profile.Id,
            profile.AccountId,
            profile.Address.ToDto(),
            profile.Bio,
            profile.Image,
            profile.IsVerified,
            profile.Experience,
            profile.Rate.Amount,
            profile.Rate.Currency,
            profile.Rating,
            profile.PortfolioUrl,
            [.. profile.ForeignLanguages.Select(fl => fl.Language)],
            [.. profile.Skills.Select(s => s.ProgrammingLanguage)],
            [.. profile.Skills.Select(s => s.Area)]);
    }

    extension(IEnumerable<FreelancerProfile> profiles)
    {
        public List<FreelancerProfileDto> ToDtos() => [.. profiles.Select(fp => fp.ToDto())];
    }

    extension(CreateFreelancerProfileRequest request)
    {
        public CreateFreelancerProfileCommand ToCreateCommand(Guid accountId) => new(
            accountId,
            request.Address,
            request.Bio,
            request.Image,
            request.Experience,
            request.Amount,
            request.Currency,
            request.PortfolioUrl,
            request.ForeignLanguages,
            request.ProgrammingLanguages,
            request.Areas);
    }

    extension(UpdateFreelancerDetailRequest request)
    {
        public UpdateFreelancerDetailsCommand ToUpdateDetailsCommand(Guid id, Guid accountId) => new(
            id,
            request.ForeignLanguages,
            request.ProgrammingLanguages,
            request.Areas,
            request.Experience,
            request.Amount,
            request.Currency,
            request.PortfolioUrl,
            accountId);
    }

    extension(UpdateFreelancerProfileRatingRequest request)
    {
        public UpdateFreelancerRatingCommand ToUpdateRatingCommand(Guid id) => new(id, request.Rating);
    }

    extension(UpdateProfileAddressRequest request)
    {
        public UpdateFreelancerProfileAddressCommand ToUpdateFreelancerAddressCommand(Guid id, Guid accountId) =>
            new(id, request.AddressDto, accountId);
    }

    extension(UpdateProfileDataRequest request)
    {
        public UpdateFreelancerProfileDataCommand ToUpdateFreelancerDataCommand(Guid id, Guid accountId) =>
            new(id, request.Bio, request.Image, accountId);
    }

    extension(VerifyProfileRequest request)
    {
        public VerifyFreelancerProfileCommand ToVerifyFreelancerCommand(Guid accountId) => new(accountId, request.ImageUrl);
    }
}
