using Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Commands;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Shared.Application.Mediator;
using Shared.Domain.Interfaces;
namespace Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Handlers;

public class DeleteClientProfileCommandHandler : IRequestHandler<DeleteClientProfileCommand, Unit>
{
    private readonly IClientProfileRepository _clientProfileRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteClientProfileCommandHandler> _logger;

    public DeleteClientProfileCommandHandler(IClientProfileRepository clientProfileRepository, IUnitOfWork unitOfWork,ILogger<DeleteClientProfileCommandHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(clientProfileRepository, nameof(clientProfileRepository));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        _clientProfileRepository = clientProfileRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeleteClientProfileCommand request, CancellationToken cancellationToken)
    {
        var clientProfile = await _clientProfileRepository.GetByIdAsync(request.Id, cancellationToken); 
         if(clientProfile is null)
        {
            _logger.LogError("Client Profile with id {ClientProfileId} not found", request.Id);
            throw new ProfileNotFoundException($"Client Profile with id {request.Id} does not exists");
        }   

        _clientProfileRepository.Delete(clientProfile);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Client Profile with id {ClientProfileId} deleted successfully", request.Id);
        return Unit.Value;
    }
}