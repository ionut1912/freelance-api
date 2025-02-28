using Frelance.Application.Mediatr.Queries.UserProfile;
using Frelance.Application.Repositories;
using Frelance.Contracts.Enums;
using MediatR;

namespace Frelance.Application.Mediatr.Handlers.UserProfile;

public class GetUserProfilesQueryHandler:IRequestHandler<GetUserProfilesQuery,object>
{
    private readonly IFreelancerProfileRepository _freelancerProfileRepository;
    private readonly IClientProfileRepository _clientProfileRepository;

    public GetUserProfilesQueryHandler(IFreelancerProfileRepository freelancerProfileRepository, 
        IClientProfileRepository clientProfileRepository)
    {
        ArgumentNullException.ThrowIfNull(freelancerProfileRepository, nameof(freelancerProfileRepository));
        ArgumentNullException.ThrowIfNull(clientProfileRepository, nameof(clientProfileRepository));
        _freelancerProfileRepository = freelancerProfileRepository;
        _clientProfileRepository = clientProfileRepository;
    }
    
    public async Task<object> Handle(GetUserProfilesQuery request, CancellationToken cancellationToken)
    {
        var repoTask = request.Role switch
        {
            Role.Client => _clientProfileRepository
                .GetClientProfilesAsync(request.PaginationParams, cancellationToken)
                .ContinueWith(object (t) => t.Result, cancellationToken),
            Role.Freelancer => _freelancerProfileRepository
                .GetAllFreelancerProfilesAsync(request.PaginationParams, cancellationToken)
                .ContinueWith(object (t) => t.Result, cancellationToken),
            _ => throw new InvalidOperationException("Invalid request")
        };
        return await repoTask;
    }
}