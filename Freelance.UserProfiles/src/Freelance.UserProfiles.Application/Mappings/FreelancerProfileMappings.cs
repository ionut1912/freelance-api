using Freelance.UserProfiles.Application.Dtos;
using Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Commands;
using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;
using Freelance.UserProfiles.Application.Requests;
using Freelance.UserProfiles.Domain.Entities;

namespace Freelance.UserProfiles.Application.Mappings;

public static class FreelancerProfileMappings
{
    public static FreelancerProfileDto ToDto(this FreelancerProfile freelancerProfile)
    {
        return new FreelancerProfileDto
        (freelancerProfile.Id,
            freelancerProfile.AccountId,
            new AddressDto(freelancerProfile.Address.Street, freelancerProfile.Address.City, freelancerProfile.Address.Country, freelancerProfile.Address.StreetNumber, freelancerProfile.Address.ZipCode, freelancerProfile.Address.State),
            freelancerProfile.Bio,
            freelancerProfile.Image,
            freelancerProfile.IsVerified,
            freelancerProfile.Experience,
            freelancerProfile.Rate.Amount,
            freelancerProfile.Rate.Currency,
            freelancerProfile.Rating,
            freelancerProfile.PortfolioUrl,
            [.. freelancerProfile.ForeignLanguages.Select(fl => fl.Language)],
            [.. freelancerProfile.Skills.Select(s => s.ProgrammingLanguage)],
            [.. freelancerProfile.Skills.Select(s => s.Area)]
        );
    }

    public static List<FreelancerProfileDto> ToDtos(this IEnumerable<FreelancerProfile> freelancerProfiles)
        => [.. freelancerProfiles.Select(fp => fp.ToDto())];

    public static CreateFreelancerProfileCommand ToCreateCommand(this CreateFreelancerProfileRequest request, Guid accountId)
    {
        return new CreateFreelancerProfileCommand(
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
            request.Areas
        );
    }

    public static UpdateFreelancerProfileAddressCommand ToUpdateFreelancerAddressCommand(this UpdateProfileAddressRequest updateFreelancerProfileAddressRequest, Guid id)
    {
        return new UpdateFreelancerProfileAddressCommand(id, updateFreelancerProfileAddressRequest.AddressDto);
    }

    public static UpdateFreelancerProfileDataCommand ToUpdateFreelancerDataCommand(this UpdateProfileDataRequest updateProfileDataRequest, Guid id)
    {
        return new UpdateFreelancerProfileDataCommand(id, updateProfileDataRequest.Bio, updateProfileDataRequest.Image);
    }

    public static UpdateFreelancerDetailsCommand ToUpdateDetailsCommand(this UpdateFreelancerDetailRequest updateFreelancerProfileDetailsRequest, Guid id)
    {
        return new UpdateFreelancerDetailsCommand(
            id,
            updateFreelancerProfileDetailsRequest.ForeignLanguages,
            updateFreelancerProfileDetailsRequest.ProgrammingLanguages,
            updateFreelancerProfileDetailsRequest.Areas,
            updateFreelancerProfileDetailsRequest.Experience,
            updateFreelancerProfileDetailsRequest.Amount,
            updateFreelancerProfileDetailsRequest.Currency,
            updateFreelancerProfileDetailsRequest.PortfolioUrl
        );
    }

    public static UpdateFreelancerRatingCommand ToUpdateRatingCommand(this UpdateFreelancerProfileRatingRequest updateProfileDataRequest, Guid id)
    {
        return new UpdateFreelancerRatingCommand(id, updateProfileDataRequest.Rating);
    }

    public static VerifyFreelancerProfileCommand ToVerifyFreelancerCommand(this VerifyProfileRequest verifyProfileRequest, Guid accountId)
    {
        return new VerifyFreelancerProfileCommand(accountId, verifyProfileRequest.ImageUrl);
    }


}
