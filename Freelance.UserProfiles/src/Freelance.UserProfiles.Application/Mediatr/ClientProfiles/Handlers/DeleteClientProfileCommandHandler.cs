using Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Commands;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Shared.Domain.Interfaces;

namespace Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Handlers;

public sealed class DeleteClientProfileCommandHandler(
    IClientProfileRepository clientProfileRepository,
    IUnitOfWork unitOfWork,
    ILogger<DeleteClientProfileCommandHandler> logger)
{
    public async Task Handle(DeleteClientProfileCommand command, CancellationToken cancellationToken)
    {
        var clientProfile = await clientProfileRepository.GetByIdAsync(command.Id, cancellationToken);
        // Someone else's profile is reported as missing, so ids cannot be probed.
        if (clientProfile?.AccountId != command.AccountId)
            throw new ProfileNotFoundException($"Client Profile with id {command.Id} does not exist");

        clientProfileRepository.Delete(clientProfile);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Client profile {ProfileId} was deleted", command.Id);
    }
}
