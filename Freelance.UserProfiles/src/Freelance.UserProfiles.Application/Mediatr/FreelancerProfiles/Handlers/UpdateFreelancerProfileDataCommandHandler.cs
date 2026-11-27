using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Shared.Domain.Interfaces;

namespace Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Handlers;

public sealed class UpdateFreelancerProfileDataCommandHandler(
    IFreelancerProfileRepository freelancerProfileRepository,
    IUnitOfWork unitOfWork,
    ILogger<UpdateFreelancerProfileDataCommandHandler> logger)
{
    public async Task Handle(UpdateFreelancerProfileDataCommand command, CancellationToken cancellationToken)
    {
        var profile = await freelancerProfileRepository.GetByIdWithTrackingAsync(command.Id, cancellationToken)
                      ?? throw new ProfileNotFoundException($"Profile with id {command.Id} not found");

        profile.UpdateUserData(command.Image, command.Bio);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Data of freelancer profile {ProfileId} was updated", command.Id);
    }
}
