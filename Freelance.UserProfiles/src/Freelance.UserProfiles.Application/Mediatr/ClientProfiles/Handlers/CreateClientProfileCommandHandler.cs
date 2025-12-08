using Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Commands;
using Freelance.UserProfiles.Domain.Entities;
using Freelance.UserProfiles.Domain.Interfaces;
using Shared.Application.Mediator;
namespace Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Handlers;

public class CreateClientProfileCommandHandler : IRequestHandler<CreateClientProfileCommand, ClientProfile>
{
    private readonly IClientProfileRepository _repository;

    public CreateClientProfileCommandHandler(IClientProfileRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository, nameof(repository));
        _repository = repository;
    }

    public async Task<ClientProfile> Handle(CreateClientProfileCommand request, CancellationToken cancellationToken)
    {
        var clientProfile = ClientProfile.Create(request.AccountId, request.AddressDto.Street, request.AddressDto.City,
            request.AddressDto.State, request.AddressDto.ZipCode, request.AddressDto.Country, request
                .AddressDto.StreetNumber, request.Bio, request.Image);
        await _repository.CreateClientProfileAsync(clientProfile, cancellationToken);
        return clientProfile;
    }
}