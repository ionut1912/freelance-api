using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.Domain.Interfaces;
using Freelancer.UserProfiles.Application.Mediatr.ClientProfiles.Commands;
using MediatR;
using Shared.Rabbit.Repositories;

namespace Freelancer.UserProfiles.Application.Mediatr.ClientProfiles.Handlers;

public class UpdateClientProfileAddressCommandHandler : IRequestHandler<UpdateClientProfileAddressCommand, Unit>
{
    private readonly IClientProfileRepository _clientProfileRepository;

    public UpdateClientProfileAddressCommandHandler(IClientProfileRepository clientProfileRepository)
    {
        ArgumentNullException.ThrowIfNull(clientProfileRepository, nameof(clientProfileRepository));
        _clientProfileRepository = clientProfileRepository;
    }

    public async Task<Unit> Handle(UpdateClientProfileAddressCommand request, CancellationToken cancellationToken)
    {
        var clientProfile = await _clientProfileRepository.GetClientProfileByIdAsync(request.Id, cancellationToken);
        if (clientProfile == null)
            throw new ProfileNotFoundException($"Client Profile with id {request.Id} does not exists");
        clientProfile.UpdateAddress(request.AddressDto.Street, request.AddressDto.City, request.AddressDto.State,
            request.AddressDto.ZipCode, request.AddressDto.Country, request.AddressDto.StreetNumber);
        await _clientProfileRepository.UpdateClientProfileAsync(clientProfile, cancellationToken);
        return Unit.Value;
    }
}