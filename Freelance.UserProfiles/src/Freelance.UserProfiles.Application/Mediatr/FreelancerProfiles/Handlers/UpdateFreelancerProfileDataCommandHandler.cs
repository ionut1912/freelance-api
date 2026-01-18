using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.Domain.Interfaces;
using Freelance.UserProfiles.Infrastructure.Persistance;
using Microsoft.Extensions.Logging;
using Shared.Application.Mediator;
using Shared.Domain.Interfaces;

namespace Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Handlers;

public class UpdateFreelancerProfileDataCommandHandler : IRequestHandler<UpdateFreelancerProfileDataCommand, Unit>
{
    private readonly IFreelancerProfileRepository _freelancerProfileRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateFreelancerProfileDataCommandHandler> _logger;

    public UpdateFreelancerProfileDataCommandHandler(IFreelancerProfileRepository freelancerProfileRepository, IUnitOfWork unitOfWork,ILogger<UpdateFreelancerProfileDataCommandHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(freelancerProfileRepository, nameof(freelancerProfileRepository));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        _freelancerProfileRepository = freelancerProfileRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Unit> Handle(UpdateFreelancerProfileDataCommand request, CancellationToken cancellationToken)
    {
        var freelancerProfile =
           await _freelancerProfileRepository.GetByIdAsync(request.Id, cancellationToken, s => s.Skills, fL => fL.ForeignLanguages);
        try
        {
            if (freelancerProfile is null)
            {
                _logger.LogError("Profile with id {ProfileId} not found", request.Id);
                throw new ProfileNotFoundException($"Profile with id {request.Id} not found");
            }


            freelancerProfile.UpdateUserData(request.Image, request.Bio);
            _freelancerProfileRepository.Update(freelancerProfile);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (ImageAlreadyExistsException ex)
        {
            _logger.LogError(ex, "Image already exists for profile with id {ProfileId}", request.Id);
            throw;
        }
        catch(BioAlreadyExistsException ex)
        {
            _logger.LogError(ex, "Bio already exists for profile with id {ProfileId}", request.Id);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating profile with id {ProfileId}", request.Id);
            throw;
        }
        _logger.LogInformation("Profile with id {ProfileId} updated successfully, {newBio},{newImage}", request.Id,freelancerProfile.Bio,freelancerProfile.Image);
        return Unit.Value;
    }
}