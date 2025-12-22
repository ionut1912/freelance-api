using Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Commands;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.Domain.Interfaces;
using Freelance.UserProfiles.Infrastructure.Persistance;
using Shared.Application.Mediator;
using Shared.Domain.Interfaces;

namespace Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Handlers;

public class UpdateClientProfileAddressCommandHandler : IRequestHandler<UpdateClientProfileAddressCommand, Unit>
{
    private readonly IClientProfileRepository _clientProfileRepository;
    private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;

    public UpdateClientProfileAddressCommandHandler(IClientProfileRepository clientProfileRepository, IUnitOfWork<ApplicationDbContext> unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(clientProfileRepository, nameof(clientProfileRepository));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        _clientProfileRepository = clientProfileRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UpdateClientProfileAddressCommand request, CancellationToken cancellationToken)
    {
        var clientProfile = await _clientProfileRepository.GetByIdAsync(request.Id, cancellationToken);
        if (clientProfile == null)
            throw new ProfileNotFoundException($"Client Profile with id {request.Id} does not exists");
        clientProfile.UpdateAddress(request.AddressDto.Street, request.AddressDto.City, request.AddressDto.State,
            request.AddressDto.ZipCode, request.AddressDto.Country, request.AddressDto.StreetNumber);
        _clientProfileRepository.Update(clientProfile);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}