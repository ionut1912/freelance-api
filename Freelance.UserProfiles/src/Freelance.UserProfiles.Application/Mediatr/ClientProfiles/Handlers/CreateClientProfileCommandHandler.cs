using Freelance.UserProfiles.Application.Dtos;
using Freelance.UserProfiles.Application.Mappings;
using Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Commands;
using Freelance.UserProfiles.Domain.Entities;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Shared.Domain.Interfaces;

namespace Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Handlers;

public sealed class CreateClientProfileCommandHandler(
    IClientProfileRepository repository,
    IUnitOfWork unitOfWork,
    ILogger<CreateClientProfileCommandHandler> logger)
{
    public async Task<ClientProfileDto> Handle(CreateClientProfileCommand command, CancellationToken cancellationToken)
    {
        if (await repository.GetLoggedInClientProfileAsync(command.AccountId, cancellationToken) is not null)
            throw new ProfileAllreadyExistsException($"Profile with accountId {command.AccountId} already exists");

        var address = command.AddressDto;
        var clientProfile = ClientProfile.Create(command.AccountId, address.Street, address.City, address.State,
            address.ZipCode, address.Country, address.StreetNumber, command.Bio, command.Image);

        await repository.AddAsync(clientProfile, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Client profile {ProfileId} was created for account {AccountId}", clientProfile.Id,
            command.AccountId);
        return clientProfile.ToDto();
    }
}
