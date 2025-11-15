using AutoMapper;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.Domain.Interfaces;
using Freelancer.UserProfiles.Application.Dtos;
using Freelancer.UserProfiles.Application.Mediatr.ClientProfiles.Queries;
using MediatR;

namespace Freelancer.UserProfiles.Application.Mediatr.ClientProfiles.Handlers;

public class GetLoggedInClientProfileQueryHandler : IRequestHandler<GetLoggedInClientProfileQuery, ClientProfileDto>
{
    private readonly IClientProfileRepository _clientProfileRepository;
    private readonly IMapper _mapper;

    public GetLoggedInClientProfileQueryHandler(IClientProfileRepository clientProfileRepository, IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(clientProfileRepository, nameof(clientProfileRepository));
        ArgumentNullException.ThrowIfNull(mapper, nameof(mapper));
        _clientProfileRepository = clientProfileRepository;
        _mapper = mapper;
    }

    public async Task<ClientProfileDto> Handle(GetLoggedInClientProfileQuery request,
        CancellationToken cancellationToken)
    {
        var clientProfile =
            await _clientProfileRepository.GetLoggedInFreelancerProfileAsync(request.AccountId, cancellationToken);
        if (clientProfile == null)
            throw new ProfileNotFoundException($"Profile with accountId {request.AccountId} not found");

        var clientProfileDto = _mapper.Map<ClientProfileDto>(clientProfile);
        return clientProfileDto;
    }
}