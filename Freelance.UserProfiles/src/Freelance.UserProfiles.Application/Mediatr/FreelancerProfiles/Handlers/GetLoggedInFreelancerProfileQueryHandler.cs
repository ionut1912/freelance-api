using Freelance.UserProfiles.Application.Dtos;
using Freelance.UserProfiles.Application.Mappings;
using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Queies;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Shared.Application.Mediator;
using System.Text.Json;

namespace Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Handlers;

public class GetLoggedInFreelancerProfileQueryHandler : IRequestHandler<GetLoggedInFreelancerProfileQuery, FreelancerProfileDto>
{
    private readonly IFreelancerProfileRepository _freelancerProfileRepository;
    private readonly ILogger<GetLoggedInFreelancerProfileQueryHandler> _logger;

    public GetLoggedInFreelancerProfileQueryHandler(IFreelancerProfileRepository freelancerProfileRepository,ILogger<GetLoggedInFreelancerProfileQueryHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(freelancerProfileRepository, nameof(freelancerProfileRepository));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        _freelancerProfileRepository = freelancerProfileRepository;
        _logger = logger;
    }

    public async Task<FreelancerProfileDto> Handle(GetLoggedInFreelancerProfileQuery request,
        CancellationToken cancellationToken)
    {
        var freelancerProfile =
            await _freelancerProfileRepository.GetLoggedInFreelancerProfileAsync(request.AccountId, cancellationToken);
        if (freelancerProfile == null)
        {
            _logger.LogError("Freelancer Profile with AccountId {AccountId} not found", request.AccountId);
            throw new ProfileNotFoundException($"Freelancer Profile with AccountId {request.AccountId} not found");
        }

        var freelancerProfileDto = freelancerProfile.ToDto();
        _logger.LogInformation("Freelancer Profile with AccountId {AccountId} retrieved successfully {freelancerProfile}", request.AccountId,
                                JsonSerializer.Serialize(freelancerProfileDto,new JsonSerializerOptions { WriteIndented=true}));
        return freelancerProfileDto;
    }
}