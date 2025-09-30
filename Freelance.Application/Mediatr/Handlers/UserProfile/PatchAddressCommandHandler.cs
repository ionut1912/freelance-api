using Freelance.Application.Mediatr.Commands.UserProfile;
using Freelance.Application.Repositories;
using Freelance.Contracts.Enums;
using MediatR;

namespace Freelance.Application.Mediatr.Handlers.UserProfile;

public class PatchAddressCommandHandler : IRequestHandler<PatchAddressCommand>
{
    private readonly IClientProfileRepository _clientProfileRepository;
    private readonly IFreelancerProfileRepository _freelancerProfileRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PatchAddressCommandHandler(IClientProfileRepository clientProfileRepository,
        IFreelancerProfileRepository freelancerProfileRepository, IUnitOfWork unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(clientProfileRepository, nameof(clientProfileRepository));
        ArgumentNullException.ThrowIfNull(freelancerProfileRepository, nameof(freelancerProfileRepository));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        _clientProfileRepository = clientProfileRepository;
        _freelancerProfileRepository = freelancerProfileRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(PatchAddressCommand request, CancellationToken cancellationToken)
    {
        var repoTask = request.Role switch
        {
            Role.Freelancer => _freelancerProfileRepository.PatchAddressAsync(request, cancellationToken),
            Role.Client => _clientProfileRepository.PatchAddressAsync(request, cancellationToken),
            _ => throw new InvalidOperationException("Invalid request")
        };

        await repoTask;
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}