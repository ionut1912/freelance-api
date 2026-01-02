using Freelance.UserProfiles.Application.Dtos;
using Freelance.UserProfiles.Application.Mappings;
using Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Queries;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.Domain.Interfaces;
using Shared.Application.Mediator;
namespace Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Handlers;

public class GetLoggedInClientProfileQueryHandler : IRequestHandler<GetLoggedInClientProfileQuery, ClientProfileDto>
{
    private readonly IClientProfileRepository _clientProfileRepository;

    public GetLoggedInClientProfileQueryHandler(IClientProfileRepository clientProfileRepository)
    {
        ArgumentNullException.ThrowIfNull(clientProfileRepository, nameof(clientProfileRepository));
        _clientProfileRepository = clientProfileRepository;
    }

    public async Task<ClientProfileDto> Handle(GetLoggedInClientProfileQuery request,
        CancellationToken cancellationToken)
    {
        var clientProfile =
            await _clientProfileRepository.GetLoggedInClientProfileAsync(request.AccountId, cancellationToken);
        if (clientProfile == null)
            throw new ProfileNotFoundException($"Profile with accountId {request.AccountId} not found");

        var clientProfileDto = clientProfile.ToDto();
        return clientProfileDto;
    }
}