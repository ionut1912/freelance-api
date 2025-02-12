using Frelance.Application.Mediatr.Commands.FreelancerProfiles;
using Frelance.Application.Repositories;
using MediatR;

namespace Frelance.Application.Mediatr.Handlers.FreelancerProfiles;

public class UpdateFreelancerProfileCommandHandler : IRequestHandler<UpdateFreelancerProfileCommand, Unit>
{
    private readonly IFreelancerProfileRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateFreelancerProfileCommandHandler(IFreelancerProfileRepository repository, IUnitOfWork unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(repository, nameof(repository));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UpdateFreelancerProfileCommand request, CancellationToken cancellationToken)
    {
        await _repository.UpdateFreelancerProfileAsync(request, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}