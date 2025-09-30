using Freelance.Application.Mediatr.Commands.UserProfile;
using Freelance.Application.Repositories;
using Freelance.Contracts.Enums;
using MediatR;

namespace Freelance.Application.Mediatr.Handlers.UserProfile;

public class VerifyUserProfileCommandHandler : IRequestHandler<VerifyUserProfileCommand>
{
    private readonly IClientProfileRepository _clientProfileRepository;
    private readonly IFreelancerProfileRepository _freelancerProfileRepository;
    private readonly IUnitOfWork _unitOfWork;

    public VerifyUserProfileCommandHandler(IFreelancerProfileRepository freelancerProfileRepository,
        IClientProfileRepository clientProfileRepository,
        IUnitOfWork unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(freelancerProfileRepository, nameof(freelancerProfileRepository));
        ArgumentNullException.ThrowIfNull(clientProfileRepository, nameof(clientProfileRepository));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        _freelancerProfileRepository = freelancerProfileRepository;
        _clientProfileRepository = clientProfileRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(VerifyUserProfileCommand request, CancellationToken cancellationToken)
    {
        var repoTask = request.Role switch
        {
            Role.Freelancer => _freelancerProfileRepository.VerifyProfileAsync(request.Id, cancellationToken),
            Role.Client => _clientProfileRepository.VerifyProfileAsync(request.Id, cancellationToken),
            _ => throw new InvalidOperationException("Invalid request")
        };

        await repoTask;
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}