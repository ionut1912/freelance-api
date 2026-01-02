using Freelance.Shared.Events.Events;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.Domain.Interfaces;
using Freelance.UserProfiles.Infrastructure.Persistance;
using Microsoft.Extensions.Logging;
using Shared.Domain.Interfaces;
using Shared.Rabbit.Repositories;

namespace Freelance.UserProfiles.Application.EventHandlers;

public class VerifiedFaceEventHandler : IEventHandler<VerifiedFaceEvent>
{
    private readonly IFreelancerProfileRepository _freelancerProfileRepository;
    private readonly IClientProfileRepository _clientProfileRepository;
    private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;
    private readonly ILogger<VerifiedFaceEventHandler> _logger;
    public VerifiedFaceEventHandler(IFreelancerProfileRepository freelancerProfileRepository, IClientProfileRepository clientProfileRepository, IUnitOfWork<ApplicationDbContext> unitOfWork, ILogger<VerifiedFaceEventHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(freelancerProfileRepository, nameof(freelancerProfileRepository));
        ArgumentNullException.ThrowIfNull(clientProfileRepository, nameof(clientProfileRepository));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        _freelancerProfileRepository = freelancerProfileRepository;
        _clientProfileRepository = clientProfileRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(VerifiedFaceEvent @event)
    {
        if (!@event.IsMatch)
        {
            _logger.LogError("Face verification failed for ProfileId: {ProfileId}", @event.ProfileId);
            throw new FaceNotMatchException("Face verification failed. The faces do not match.");
        }

        if(@event.Role== "Client")
        {
            var clientProfile =  await _clientProfileRepository.GetByIdAsync(@event.ProfileId);
            if(clientProfile == null)
            {
                _logger.LogError("ClientProfile not found for ProfileId: {ProfileId}", @event.ProfileId);
                throw new ProfileNotFoundException($"ClientProfile with ID {@event.ProfileId} not found.");
            }

            clientProfile.Verify();
            _clientProfileRepository.Update(clientProfile);
        }
        else if(@event.Role== "Freelancer")
        {
            var freelancerProfile =  await _freelancerProfileRepository.GetByIdAsync(@event.ProfileId);
            if(freelancerProfile == null)
            {
                _logger.LogError("FreelancerProfile not found for ProfileId: {ProfileId}", @event.ProfileId);
                throw new ProfileNotFoundException($"FreelancerProfile with ID {@event.ProfileId} not found.");
            }
            freelancerProfile.Verify();
            _freelancerProfileRepository.Update(freelancerProfile);
        }

        await _unitOfWork.SaveChangesAsync(CancellationToken.None);
    }
}