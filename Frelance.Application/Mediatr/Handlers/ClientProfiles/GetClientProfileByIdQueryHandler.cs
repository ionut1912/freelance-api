using Frelance.Application.Mediatr.Queries.ClientProfiles;
using Frelance.Application.Repositories;
using Frelance.Contracts.Dtos;
using MediatR;

namespace Frelance.Application.Mediatr.Handlers.ClientProfiles;

public class GetClientProfileByIdQueryHandler : IRequestHandler<GetClientProfileByIdQuery, ClientProfileDto>
{
    private readonly IClientProfileRepository _clientProfileRepository;

    public GetClientProfileByIdQueryHandler(IClientProfileRepository clientProfileRepository)
    {
        ArgumentNullException.ThrowIfNull(clientProfileRepository, nameof(clientProfileRepository));
        _clientProfileRepository = clientProfileRepository;
    }
    public async Task<ClientProfileDto> Handle(GetClientProfileByIdQuery request, CancellationToken cancellationToken)
    {
        return await _clientProfileRepository.GetClientProfileByIdAsync(request, cancellationToken);
    }
}