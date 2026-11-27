using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Shared.Domain.Interfaces;

namespace Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Handlers;

public sealed class UpdateFreelancerRatingCommandHandler(
    IFreelancerProfileRepository freelancerProfileRepository,
    IUnitOfWork unitOfWork,
    ILogger<UpdateFreelancerRatingCommandHandler> logger)
{
    public async Task Handle(UpdateFreelancerRatingCommand command, CancellationToken cancellationToken)
    {
        var profile = await freelancerProfileRepository.GetByIdWithTrackingAsync(command.Id, cancellationToken)
                      ?? throw new ProfileNotFoundException($"Profile with id {command.Id} not found");

        profile.UpdateRating(command.Rating);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Freelancer profile {ProfileId} rated {Rating}", command.Id, command.Rating);
    }
}
