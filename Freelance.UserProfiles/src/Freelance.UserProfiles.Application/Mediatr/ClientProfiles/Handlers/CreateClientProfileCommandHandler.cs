using Freelance.UserProfiles.Domain.Entities;
using Freelance.UserProfiles.Domain.Interfaces;
using Freelancer.UserProfiles.Application.Mediatr.ClientProfiles.Commands;
using MediatR;

namespace Freelancer.UserProfiles.Application.Mediatr.ClientProfiles.Handlers;

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
        var clientProfile = ClientProfile.Create(request.AccountId, request.Address.Street, request.Address.City,
            request.Address.State, request.Address.ZipCode, request.Address.Country, request
                .Address.StreetNumber, request.Bio, request.Image);
        await _repository.CreateClientProfileAsync(clientProfile, cancellationToken);
        return clientProfile;
    }
}