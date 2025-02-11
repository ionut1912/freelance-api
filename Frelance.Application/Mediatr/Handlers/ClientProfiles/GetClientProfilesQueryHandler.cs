using Frelance.Application.Mediatr.Queries.ClientProfiles;
using Frelance.Application.Repositories;
using Frelance.Contracts.Dtos;
using Frelance.Contracts.Responses.Common;
using MediatR;

namespace Frelance.Application.Mediatr.Handlers.ClientProfiles;

public class GetClientProfilesQueryHandler : IRequestHandler<GetClientProfilesQuery, PaginatedList<ClientProfileDto>>
{
    private readonly IClientProfileRepository _clientProfileRepository;

    public GetClientProfilesQueryHandler(IClientProfileRepository clientProfileRepository)
    {
        ArgumentNullException.ThrowIfNull(clientProfileRepository, nameof(clientProfileRepository));
        _clientProfileRepository = clientProfileRepository;
    }

    public async Task<PaginatedList<ClientProfileDto>> Handle(GetClientProfilesQuery request, CancellationToken cancellationToken)
    {
        return await _clientProfileRepository.GetClientProfilesAsync(request, cancellationToken);
    }
}