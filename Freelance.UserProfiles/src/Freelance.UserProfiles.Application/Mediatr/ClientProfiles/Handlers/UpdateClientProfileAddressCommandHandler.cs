using Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Commands;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Shared.Domain.Interfaces;

namespace Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Handlers;

public sealed class UpdateClientProfileAddressCommandHandler(
    IClientProfileRepository clientProfileRepository,
    IUnitOfWork unitOfWork,
    ILogger<UpdateClientProfileAddressCommandHandler> logger)
{
    public async Task Handle(UpdateClientProfileAddressCommand command, CancellationToken cancellationToken)
    {
        var clientProfile = await clientProfileRepository.GetByIdAsync(command.Id, cancellationToken);
        // Someone else's profile is reported as missing, so ids cannot be probed.
        if (clientProfile?.AccountId != command.AccountId)
            throw new ProfileNotFoundException($"Client Profile with id {command.Id} does not exist");

        var address = command.AddressDto;
        clientProfile.UpdateAddress(address.Street, address.City, address.State, address.ZipCode, address.Country,
            address.StreetNumber);
        clientProfileRepository.Update(clientProfile);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Address of client profile {ProfileId} was updated", command.Id);
    }
}
