using Frelance.Application.Mediatr.Queries.UserProfile;
using Frelance.Application.Repositories;
using Frelance.Contracts.Enums;
using Frelance.Contracts.Responses.Common;
using MediatR;

namespace Frelance.Application.Mediatr.Handlers.UserProfile;

public class GetUserProfilesQueryHandler : IRequestHandler<GetUserProfilesQuery, PaginatedList<object>>
{
    private readonly IClientProfileRepository _clientProfileRepository;
    private readonly IFreelancerProfileRepository _freelancerProfileRepository;

    public GetUserProfilesQueryHandler(IFreelancerProfileRepository freelancerProfileRepository,
        IClientProfileRepository clientProfileRepository)
    {
        ArgumentNullException.ThrowIfNull(freelancerProfileRepository, nameof(freelancerProfileRepository));
        ArgumentNullException.ThrowIfNull(clientProfileRepository, nameof(clientProfileRepository));
        _freelancerProfileRepository = freelancerProfileRepository;
        _clientProfileRepository = clientProfileRepository;
    }

    public async Task<PaginatedList<object>> Handle(GetUserProfilesQuery request, CancellationToken cancellationToken)
    {
        switch (request.Role)
        {
            case Role.Client:
                {
                    var clientProfiles =
                        await _clientProfileRepository.GetClientProfilesAsync(request.PaginationParams, cancellationToken);
                    return new PaginatedList<object>(
                        clientProfiles.Items.Cast<object>().ToList(),
                        clientProfiles.TotalCount,
                        clientProfiles.CurrentPage,
                        clientProfiles.PageSize);
                }
            case Role.Freelancer:
                {
                    var freelancerProfiles =
                        await _freelancerProfileRepository.GetAllFreelancerProfilesAsync(request.PaginationParams,
                            cancellationToken);
                    return new PaginatedList<object>(
                        freelancerProfiles.Items.Cast<object>().ToList(),
                        freelancerProfiles.TotalCount,
                        freelancerProfiles.CurrentPage,
                        freelancerProfiles.PageSize);
                }
            default:
                throw new InvalidOperationException("Invalid request");
        }
    }
}