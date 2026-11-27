using Freelance.UserProfiles.Application.Dtos;
using Freelance.UserProfiles.Application.Mappings;
using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;
using Freelance.UserProfiles.Domain.Entities;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.Domain.Interfaces;
using Freelance.UserProfiles.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Shared.Domain.Interfaces;

namespace Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Handlers;

public sealed class CreateFreelancerProfileCommandHandler(
    IFreelancerProfileRepository freelancerProfileRepository,
    ISkillRepository skillRepository,
    IUnitOfWork unitOfWork,
    ILogger<CreateFreelancerProfileCommandHandler> logger)
{
    public async Task<FreelancerProfileDto> Handle(CreateFreelancerProfileCommand command,
        CancellationToken cancellationToken)
    {
        if (await freelancerProfileRepository.GetLoggedInFreelancerProfileAsync(command.AccountId, cancellationToken) is not null)
            throw new ProfileAllreadyExistsException($"Profile with accountId {command.AccountId} already exists");

        var address = command.Address;
        var profile = FreelancerProfile.Create(command.AccountId, address.Street, address.City, address.State,
            address.ZipCode, address.Country, address.StreetNumber, command.Bio, command.Image, command.Experience,
            command.Amount, command.Currency, rating: 0, command.PortfolioUrl);

        profile.AddLanguages(command.ForeignLanguages.Select(FreelancerForeignLanguage.Create));
        profile.AddSkills(await skillRepository.ResolveSkillsAsync(command.ProgrammingLanguages, command.Areas,
            cancellationToken));

        await freelancerProfileRepository.AddAsync(profile, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Freelancer profile {ProfileId} was created for account {AccountId}", profile.Id,
            command.AccountId);
        return profile.ToDto();
    }
}
