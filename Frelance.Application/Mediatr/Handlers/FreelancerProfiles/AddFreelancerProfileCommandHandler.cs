using Frelance.Application.Mediatr.Commands.FreelancerProfiles;
using Frelance.Application.Repositories;
using MediatR;

namespace Frelance.Application.Mediatr.Handlers.FreelancerProfiles;

public class AddFreelancerProfileCommandHandler : IRequestHandler<AddFreelancerProfileCommand, Unit>
{
    private readonly IFreelancerProfileRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public AddFreelancerProfileCommandHandler(IFreelancerProfileRepository repository, IUnitOfWork unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(repository, nameof(repository));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(AddFreelancerProfileCommand request, CancellationToken cancellationToken)
    {
        await _repository.AddFreelancerProfileAsync(request, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}