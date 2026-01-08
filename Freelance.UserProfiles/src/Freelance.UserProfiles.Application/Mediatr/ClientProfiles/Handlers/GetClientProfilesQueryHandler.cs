using Freelance.UserProfiles.Application.Dtos;
using Freelance.UserProfiles.Application.Mappings;
using Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Queries;
using Freelance.UserProfiles.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Shared.Application.Mediator;
using System.Text.Json;
namespace Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Handlers;

public class GetClientProfilesQueryHandler : IRequestHandler<GetClientProfilesQuery, List<ClientProfileDto>>
{
    private readonly IClientProfileRepository _clientProfileRepository;
    private readonly ILogger<GetClientProfilesQueryHandler> _logger;

    public GetClientProfilesQueryHandler(IClientProfileRepository clientProfileRepository, ILogger<GetClientProfilesQueryHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(clientProfileRepository, nameof(clientProfileRepository));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        _clientProfileRepository = clientProfileRepository;
        _logger = logger;
    }

    public async Task<List<ClientProfileDto>> Handle(GetClientProfilesQuery request,
        CancellationToken cancellationToken)
    {
        var clientProfiles = await _clientProfileRepository.GetAllAsync(cancellationToken);
        var clientDtos = clientProfiles.ToDtos();
        _logger.LogInformation("Client profiles {clientDtos}", JsonSerializer.Serialize(clientDtos, new JsonSerializerOptions { WriteIndented = true }));
        return clientDtos;
    }
}