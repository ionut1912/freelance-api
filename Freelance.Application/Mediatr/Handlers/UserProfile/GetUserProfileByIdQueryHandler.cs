using Freelance.Application.Mediatr.Queries.UserProfile;
using Freelance.Application.Repositories;
using Freelance.Contracts.Enums;
using MediatR;

namespace Freelance.Application.Mediatr.Handlers.UserProfile;

public class GetUserProfileByIdQueryHandler : IRequestHandler<GetUserProfileByIdQuery, object>
{
    private readonly IClientProfileRepository _clientProfileRepository;
    private readonly IFreelancerProfileRepository _freelancerProfileRepository;

    public GetUserProfileByIdQueryHandler(IClientProfileRepository clientProfileRepository,
        IFreelancerProfileRepository freelancerProfileRepository)
    {
        ArgumentNullException.ThrowIfNull(clientProfileRepository, nameof(clientProfileRepository));
        ArgumentNullException.ThrowIfNull(freelancerProfileRepository, nameof(freelancerProfileRepository));
        _clientProfileRepository = clientProfileRepository;
        _freelancerProfileRepository = freelancerProfileRepository;
    }

    public async Task<object> Handle(GetUserProfileByIdQuery request, CancellationToken cancellationToken)
    {
        var repoTask = request.Role switch
        {
            Role.Client => _clientProfileRepository
                .GetClientProfileByIdAsync(request.Id, cancellationToken)
                .ContinueWith(object (t) => t.Result, cancellationToken),
            Role.Freelancer => _freelancerProfileRepository
                .GetFreelancerProfileByIdAsync(request.Id, cancellationToken)
                .ContinueWith(object (t) => t.Result, cancellationToken),
            _ => throw new InvalidOperationException("Invalid request")
        };

        return await repoTask;
    }
}