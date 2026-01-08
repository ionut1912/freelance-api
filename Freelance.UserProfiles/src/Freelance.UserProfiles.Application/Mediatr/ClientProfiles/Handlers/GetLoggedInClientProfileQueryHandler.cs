using Freelance.UserProfiles.Application.Dtos;
using Freelance.UserProfiles.Application.Mappings;
using Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Queries;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Shared.Application.Mediator;
using System.Text.Json;
namespace Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Handlers;

public class GetLoggedInClientProfileQueryHandler : IRequestHandler<GetLoggedInClientProfileQuery, ClientProfileDto>
{
    private readonly IClientProfileRepository _clientProfileRepository;
    private readonly ILogger<GetLoggedInClientProfileQueryHandler> _logger;

    public GetLoggedInClientProfileQueryHandler(IClientProfileRepository clientProfileRepository,ILogger<GetLoggedInClientProfileQueryHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(clientProfileRepository, nameof(clientProfileRepository));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        _clientProfileRepository = clientProfileRepository;
        _logger = logger;
    }

    public async Task<ClientProfileDto> Handle(GetLoggedInClientProfileQuery request,
        CancellationToken cancellationToken)
    {
        var clientProfile =
            await _clientProfileRepository.GetLoggedInClientProfileAsync(request.AccountId, cancellationToken);
        if (clientProfile == null)
        {
            _logger.LogError("Profile with accountId {AccountId} was not found", request.AccountId);
            throw new ProfileNotFoundException($"Profile with accountId {request.AccountId} not found");
        }
      
        var clientProfileDto = clientProfile.ToDto();
        _logger.LogInformation("Profile with accountId {AccountId} was found {clientProfileDto}", request.AccountId, JsonSerializer.Serialize(clientProfileDto, new JsonSerializerOptions { WriteIndented = true }));
        return clientProfileDto;
    }
}