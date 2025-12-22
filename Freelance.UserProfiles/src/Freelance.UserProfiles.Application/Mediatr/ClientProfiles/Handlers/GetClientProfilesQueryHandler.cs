using Freelance.UserProfiles.Application.Dtos;
using Freelance.UserProfiles.Application.Mappings;
using Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Queries;
using Freelance.UserProfiles.Domain.Interfaces;
using Shared.Application.Mediator;
namespace Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Handlers;

public class GetClientProfilesQueryHandler : IRequestHandler<GetClientProfilesQuery, List<ClientProfileDto>>
{
    private readonly IClientProfileRepository _clientProfileRepository;

    public GetClientProfilesQueryHandler(IClientProfileRepository clientProfileRepository)
    {
        ArgumentNullException.ThrowIfNull(clientProfileRepository, nameof(clientProfileRepository));
        _clientProfileRepository = clientProfileRepository;
    }

    public async Task<List<ClientProfileDto>> Handle(GetClientProfilesQuery request,
        CancellationToken cancellationToken)
    {
        var clientProfiles = await _clientProfileRepository.GetAllAsync(cancellationToken);
        var clientDtos = clientProfiles.ToDtos();
        return clientDtos;
    }
}