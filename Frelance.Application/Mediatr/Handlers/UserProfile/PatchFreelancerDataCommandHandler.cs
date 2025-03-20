using Frelance.Application.Mediatr.Commands.UserProfile;
using Frelance.Application.Repositories;
using MediatR;

namespace Frelance.Application.Mediatr.Handlers.UserProfile;

public class PatchFreelancerDataCommandHandler : IRequestHandler<PatchFreelancerDataCommand>
{
    private readonly IFreelancerProfileRepository _freelancerProfilesRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PatchFreelancerDataCommandHandler(IFreelancerProfileRepository freelancerProfilesRepository,
        IUnitOfWork unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(freelancerProfilesRepository, nameof(freelancerProfilesRepository));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        _freelancerProfilesRepository = freelancerProfilesRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(PatchFreelancerDataCommand request, CancellationToken cancellationToken)
    {
        await _freelancerProfilesRepository.PatchFreelancerDetailsAsync(request, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}