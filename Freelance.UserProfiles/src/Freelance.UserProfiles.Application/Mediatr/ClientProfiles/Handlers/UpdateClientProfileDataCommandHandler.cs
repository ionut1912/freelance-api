using Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Commands;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Shared.Application.Mediator;
using Shared.Domain.Interfaces;

namespace Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Handlers;

public class UpdateClientProfileDataCommandHandler : IRequestHandler<UpdateClientProfileDataCommand, Unit>
{
    private readonly IClientProfileRepository _clientProfileRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateClientProfileDataCommandHandler> _logger;

    public UpdateClientProfileDataCommandHandler(IClientProfileRepository clientProfileRepository, IUnitOfWork unitOfWork,ILogger<UpdateClientProfileDataCommandHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(clientProfileRepository, nameof(clientProfileRepository));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        _clientProfileRepository = clientProfileRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Unit> Handle(UpdateClientProfileDataCommand request, CancellationToken cancellationToken)
    {
        var clientProfile = await _clientProfileRepository.GetByIdAsync(request.Id, cancellationToken);
        try
        {
       
            if (clientProfile == null)
            {
                _logger.LogError("Client Profile with id {ClientProfileId} does not exists", request.Id);
                throw new ProfileNotFoundException($"Client Profile with id {request.Id} does not exists");
            }
 

            clientProfile.UpdateUserData(request.Image, request.Bio);
            _clientProfileRepository.Update(clientProfile);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch(ImageAlreadyExistsException ex)
        {
            _logger.LogError(ex, "Image {Image} already exists", request.Image);
            throw;
        }
        catch(BioAlreadyExistsException ex)
        {
            _logger.LogError(ex, "Bio {Bio} already exists", request.Bio);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating Client Profile data with id {ClientProfileId}", request.Id);
            throw;
        }

        _logger.LogInformation("Client Profile data with id {ClientProfileId} updated successfully,newBio {Bio},newImage {Image}", request.Id,clientProfile.Bio,clientProfile.Image);
        return Unit.Value;
    }
}