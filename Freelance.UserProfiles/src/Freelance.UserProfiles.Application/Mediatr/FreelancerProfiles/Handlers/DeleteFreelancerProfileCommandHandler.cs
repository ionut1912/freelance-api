using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Shared.Application.Mediator;
using Shared.Domain.Interfaces;

namespace Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Handlers;

public class DeleteFreelancerProfileCommandHandler : IRequestHandler<DeleteFreelancerProfileCommand, Unit>
{
    private readonly IFreelancerProfileRepository _freelancerProfileRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteFreelancerProfileCommandHandler> _logger;

    public DeleteFreelancerProfileCommandHandler(IFreelancerProfileRepository freelancerProfileRepository, IUnitOfWork unitOfWork,ILogger<DeleteFreelancerProfileCommandHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(freelancerProfileRepository, nameof(freelancerProfileRepository));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        _freelancerProfileRepository = freelancerProfileRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeleteFreelancerProfileCommand request, CancellationToken cancellationToken)
    {
        var freelancerProfile =
            await _freelancerProfileRepository.GetByIdAsync(request.Id, cancellationToken);
        if (freelancerProfile is null)
        {
            _logger.LogError("Profile with id {ProfileId} not found", request.Id);
            throw new ProfileNotFoundException($"Profile with id {request.Id} not found");
        }
        _freelancerProfileRepository.Delete(freelancerProfile);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Profile with ID {ProfileId} was deleted successfully", request.Id);
        return Unit.Value;
    }
}