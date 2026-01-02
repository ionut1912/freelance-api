using Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Commands;
using Freelance.UserProfiles.Domain.Entities;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.Domain.Interfaces;
using Freelance.UserProfiles.Infrastructure.Persistance;
using Shared.Application.Mediator;
using Shared.Domain.Interfaces;
namespace Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Handlers;

public class CreateClientProfileCommandHandler : IRequestHandler<CreateClientProfileCommand, ClientProfile>
{
    private readonly IClientProfileRepository _repository;
    private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;

    public CreateClientProfileCommandHandler(IClientProfileRepository repository, IUnitOfWork<ApplicationDbContext> unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(repository, nameof(repository));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ClientProfile> Handle(CreateClientProfileCommand request, CancellationToken cancellationToken)
    {
        var existingClientProfile=await _repository.GetLoggedInClientProfileAsync(request.AccountId, cancellationToken);
        if (existingClientProfile != null) 
        {
            throw new ProfileAllreadyExistsException($"Profile with accountId {request.AccountId} allready exists");
        }

        var clientProfile = ClientProfile.Create(request.AccountId, request.AddressDto.Street, request.AddressDto.City,
            request.AddressDto.State, request.AddressDto.ZipCode, request.AddressDto.Country, request
                .AddressDto.StreetNumber, request.Bio, request.Image);
        await _repository.AddAsync(clientProfile, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return clientProfile;
    }
}