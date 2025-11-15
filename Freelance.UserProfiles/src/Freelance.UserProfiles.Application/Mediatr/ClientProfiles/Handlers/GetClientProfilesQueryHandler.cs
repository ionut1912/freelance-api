using AutoMapper;
using Freelance.UserProfiles.Domain.Interfaces;
using Freelancer.UserProfiles.Application.Dtos;
using Freelancer.UserProfiles.Application.Mediatr.ClientProfiles.Queries;
using MediatR;

namespace Freelancer.UserProfiles.Application.Mediatr.ClientProfiles.Handlers;

public class GetClientProfilesQueryHandler : IRequestHandler<GetClientProfilesQuery, List<ClientProfileDto>>
{
    private readonly IClientProfileRepository _clientProfileRepository;
    private readonly IMapper _mapper;

    public GetClientProfilesQueryHandler(IClientProfileRepository clientProfileRepository, IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(clientProfileRepository, nameof(clientProfileRepository));
        ArgumentNullException.ThrowIfNull(mapper, nameof(mapper));
        _clientProfileRepository = clientProfileRepository;
        _mapper = mapper;
    }

    public async Task<List<ClientProfileDto>> Handle(GetClientProfilesQuery request,
        CancellationToken cancellationToken)
    {
        var clientProfiles = await _clientProfileRepository.GetClientProfilesAsync(cancellationToken);
        var clientDtos = _mapper.Map<List<ClientProfileDto>>(clientProfiles);
        return clientDtos;
    }
}