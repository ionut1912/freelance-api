using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.Domain.Interfaces;
using Freelance.UserProfiles.Infrastructure.Persistance;
using Shared.Application.Mediator;
using Shared.Domain.Interfaces;

namespace Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Handlers;

public class UpdateFreelancerProfileDataCommandHandler : IRequestHandler<UpdateFreelancerProfileDataCommand, Unit>
{
    private readonly IFreelancerProfileRepository _freelancerProfileRepository;
    private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;

    public UpdateFreelancerProfileDataCommandHandler(IFreelancerProfileRepository freelancerProfileRepository, IUnitOfWork<ApplicationDbContext> unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(freelancerProfileRepository, nameof(freelancerProfileRepository));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        _freelancerProfileRepository = freelancerProfileRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UpdateFreelancerProfileDataCommand request, CancellationToken cancellationToken)
    {
        var freelancerProfile =
            await _freelancerProfileRepository.GetByIdAsync(request.Id, cancellationToken, s => s.Skills, fL => fL.ForeignLanguages);
        if (freelancerProfile is null)
            throw new ProfileNotFoundException($"Profile with id {request.Id} not found");

        freelancerProfile.UpdateUserData(request.Image, request.Bio);
        _freelancerProfileRepository.Update(freelancerProfile);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}