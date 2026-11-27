using Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Commands;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Shared.Domain.Interfaces;

namespace Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Handlers;

public sealed class UpdateClientProfileDataCommandHandler(
    IClientProfileRepository clientProfileRepository,
    IUnitOfWork unitOfWork,
    ILogger<UpdateClientProfileDataCommandHandler> logger)
{
    public async Task Handle(UpdateClientProfileDataCommand command, CancellationToken cancellationToken)
    {
        var clientProfile = await clientProfileRepository.GetByIdAsync(command.Id, cancellationToken)
                            ?? throw new ProfileNotFoundException($"Client Profile with id {command.Id} does not exist");

        clientProfile.UpdateUserData(command.Image, command.Bio);
        clientProfileRepository.Update(clientProfile);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Data of client profile {ProfileId} was updated", command.Id);
    }
}
