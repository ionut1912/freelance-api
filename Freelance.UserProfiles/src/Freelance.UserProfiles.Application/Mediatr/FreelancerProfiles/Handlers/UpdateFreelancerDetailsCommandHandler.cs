using Freelance.UserProfiles.Application.Mappings;
using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.Domain.Interfaces;
using Freelance.UserProfiles.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Shared.Domain.Interfaces;

namespace Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Handlers;

public sealed class UpdateFreelancerDetailsCommandHandler(
    IFreelancerProfileRepository freelancerProfileRepository,
    ISkillRepository skillRepository,
    IUnitOfWork unitOfWork,
    ILogger<UpdateFreelancerDetailsCommandHandler> logger)
{
    public async Task Handle(UpdateFreelancerDetailsCommand command, CancellationToken cancellationToken)
    {
        // Tracked load so EF computes the exact inserts/deletes for the languages and the skills join table.
        var profile = await freelancerProfileRepository.GetByIdWithTrackingAsync(command.Id, cancellationToken,
                          p => p.Skills, p => p.ForeignLanguages)
                      ?? throw new ProfileNotFoundException($"Profile with id {command.Id} not found");

        profile.UpdateLanguages(command.ForeignLanguages.Select(FreelancerForeignLanguage.Create));
        profile.UpdateFreelancerDetails(command.Experience, command.Amount, command.Currency, command.PortfolioUrl);
        profile.UpdateSkills(await skillRepository.ResolveSkillsAsync(command.ProgrammingLanguages, command.Areas,
            cancellationToken));

        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Details of freelancer profile {ProfileId} were updated", command.Id);
    }
}
