using Freelance.UserProfiles.Application.Dtos;
using Freelance.UserProfiles.Application.Mappings;
using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Queies;
using Freelance.UserProfiles.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Shared.Application.Mediator;
using System.Text.Json;

namespace Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Handlers;

public class GetFreelancerProfilesQueryHandler : IRequestHandler<GetFreelancerProfilesQuery, List<FreelancerProfileDto>>
{
    private readonly IFreelancerProfileRepository _freelancerProfileRepository;
    private readonly ILogger<GetFreelancerProfilesQueryHandler> _logger;

    public GetFreelancerProfilesQueryHandler(IFreelancerProfileRepository freelancerProfileRepository,ILogger<GetFreelancerProfilesQueryHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(freelancerProfileRepository, nameof(freelancerProfileRepository));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        _freelancerProfileRepository = freelancerProfileRepository;
        _logger = logger;
    }

    public async Task<List<FreelancerProfileDto>> Handle(GetFreelancerProfilesQuery request,
        CancellationToken cancellationToken)
    {
        var freelancerProfiles = await _freelancerProfileRepository.GetAllAsync(cancellationToken, s => s.Skills, fL => fL.ForeignLanguages);
        var freelancerProfileDtos = freelancerProfiles.ToDtos();
        _logger.LogInformation("Freelancer profiles {freelancerProfileDtos}", JsonSerializer.Serialize(freelancerProfileDtos, new JsonSerializerOptions { WriteIndented = true }));
        return freelancerProfileDtos;
    }
}