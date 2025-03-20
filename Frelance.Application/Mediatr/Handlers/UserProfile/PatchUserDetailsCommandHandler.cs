using Frelance.Application.Mediatr.Commands.UserProfile;
using Frelance.Application.Repositories;
using Frelance.Contracts.Enums;
using MediatR;

namespace Frelance.Application.Mediatr.Handlers.UserProfile;

public class PatchUserDetailsCommandHandler : IRequestHandler<PatchUserDetailsCommand>
{
    private readonly IClientProfileRepository _clientProfileRepository;
    private readonly IFreelancerProfileRepository _freelancerProfileRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PatchUserDetailsCommandHandler(IFreelancerProfileRepository freelancerProfileRepository,
        IClientProfileRepository clientProfileRepository, IUnitOfWork unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(freelancerProfileRepository, nameof(freelancerProfileRepository));
        ArgumentNullException.ThrowIfNull(clientProfileRepository, nameof(clientProfileRepository));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        _freelancerProfileRepository = freelancerProfileRepository;
        _clientProfileRepository = clientProfileRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(PatchUserDetailsCommand request, CancellationToken cancellationToken)
    {
        var repoTask = request.Role switch
        {
            Role.Freelancer => _freelancerProfileRepository.PatchUserDetailsAsync(request, cancellationToken),
            Role.Client => _clientProfileRepository.PatchUserDetailsAsync(request, cancellationToken),
            _ => throw new InvalidOperationException("Invalid request")
        };

        await repoTask;
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}