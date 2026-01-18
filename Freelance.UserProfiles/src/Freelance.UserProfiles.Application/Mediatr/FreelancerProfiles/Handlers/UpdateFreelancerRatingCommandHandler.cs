using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Shared.Application.Mediator;
using Shared.Domain.Interfaces;

namespace Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Handlers;

public class UpdateFreelancerRatingCommandHandler : IRequestHandler<UpdateFreelancerRatingCommand, Unit>
{
    private readonly IFreelancerProfileRepository _freelancerProfileRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateFreelancerRatingCommandHandler> _logger;

    public UpdateFreelancerRatingCommandHandler(IFreelancerProfileRepository freelancerProfileRepository, IUnitOfWork unitOfWork,ILogger<UpdateFreelancerRatingCommandHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(freelancerProfileRepository, nameof(freelancerProfileRepository));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        _freelancerProfileRepository = freelancerProfileRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Unit> Handle(UpdateFreelancerRatingCommand request, CancellationToken cancellationToken)
    {
        var freelancerProfile =
            await _freelancerProfileRepository.GetByIdAsync(request.Id, cancellationToken, s => s.Skills, fL => fL.ForeignLanguages);
        if (freelancerProfile is null)
        {
            _logger.LogError("Profile with id {ProfileId} not found", request.Id);
            throw new ProfileNotFoundException($"Profile with id {request.Id} not found");
        }

        freelancerProfile.UpdateRating(request.Rating);
        _freelancerProfileRepository.Update(freelancerProfile);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Profile with id {ProfileId} updated with new rating {Rating}", request.Id, request.Rating);
        return Unit.Value;
    }
}