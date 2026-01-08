using Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Commands;
using Freelance.UserProfiles.Domain.Entities;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.Domain.Interfaces;
using Freelance.UserProfiles.Infrastructure.Persistance;
using Microsoft.Extensions.Logging;
using Shared.Application.Mediator;
using Shared.Domain.Interfaces;
namespace Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Handlers;

public class CreateClientProfileCommandHandler : IRequestHandler<CreateClientProfileCommand, ClientProfile>
{
    private readonly IClientProfileRepository _repository;
    private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;
    private readonly ILogger<CreateClientProfileCommandHandler> _logger;

    public CreateClientProfileCommandHandler(IClientProfileRepository repository, IUnitOfWork<ApplicationDbContext> unitOfWork, ILogger<CreateClientProfileCommandHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(repository, nameof(repository));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ClientProfile> Handle(CreateClientProfileCommand request, CancellationToken cancellationToken)
    {
        var existingClientProfile=await _repository.GetLoggedInClientProfileAsync(request.AccountId, cancellationToken);
        if (existingClientProfile != null) 
        {
            _logger.LogError("Profile with accountId {AccountId} allready exists", request.AccountId);
            throw new ProfileAllreadyExistsException($"Profile with accountId {request.AccountId} allready exists");
        }

        var clientProfile = ClientProfile.Create(request.AccountId, request.AddressDto.Street, request.AddressDto.City,
            request.AddressDto.State, request.AddressDto.ZipCode, request.AddressDto.Country, request
                .AddressDto.StreetNumber, request.Bio, request.Image);
        await _repository.AddAsync(clientProfile, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Created client profile with id {ClientProfileId} for accountId {AccountId}", clientProfile.Id, request.AccountId);
        return clientProfile;
    }
}