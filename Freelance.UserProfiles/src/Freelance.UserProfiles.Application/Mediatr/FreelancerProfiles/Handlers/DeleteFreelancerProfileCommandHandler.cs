using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Shared.Domain.Interfaces;

namespace Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Handlers;

public sealed class DeleteFreelancerProfileCommandHandler(
    IFreelancerProfileRepository freelancerProfileRepository,
    IUnitOfWork unitOfWork,
    ILogger<DeleteFreelancerProfileCommandHandler> logger)
{
    public async Task Handle(DeleteFreelancerProfileCommand command, CancellationToken cancellationToken)
    {
        var profile = await freelancerProfileRepository.GetByIdAsync(command.Id, cancellationToken)
                      ?? throw new ProfileNotFoundException($"Profile with id {command.Id} not found");

        freelancerProfileRepository.Delete(profile);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Freelancer profile {ProfileId} was deleted", command.Id);
    }
}
