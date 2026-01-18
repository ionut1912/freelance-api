using Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Commands;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Shared.Application.Mediator;
using Shared.Domain.Interfaces;
using System.Text.Json;

namespace Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Handlers;

public class UpdateClientProfileAddressCommandHandler : IRequestHandler<UpdateClientProfileAddressCommand, Unit>
{
    private readonly IClientProfileRepository _clientProfileRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateClientProfileAddressCommandHandler> _logger;

    public UpdateClientProfileAddressCommandHandler(IClientProfileRepository clientProfileRepository, IUnitOfWork unitOfWork,ILogger<UpdateClientProfileAddressCommandHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(clientProfileRepository, nameof(clientProfileRepository));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        _clientProfileRepository = clientProfileRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Unit> Handle(UpdateClientProfileAddressCommand request, CancellationToken cancellationToken)
    {
        var clientProfile = await _clientProfileRepository.GetByIdAsync(request.Id, cancellationToken);
        if (clientProfile == null)
        {
            _logger.LogInformation("Client Profile with id {ClientProfileId} was not found", request.Id);
            throw new ProfileNotFoundException($"Client Profile with id {request.Id} does not exists");
        }
        clientProfile.UpdateAddress(request.AddressDto.Street, request.AddressDto.City, request.AddressDto.State,
            request.AddressDto.ZipCode, request.AddressDto.Country, request.AddressDto.StreetNumber);
        _clientProfileRepository.Update(clientProfile);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Client Profile Address for profile with id {ClientProfileId} was updated successfully {Address}", request.Id,JsonSerializer.Serialize(clientProfile.Address,new JsonSerializerOptions { WriteIndented=true}));
        return Unit.Value;
    }
}