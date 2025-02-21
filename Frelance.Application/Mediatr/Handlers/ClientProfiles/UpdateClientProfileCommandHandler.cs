using Frelance.Application.Mediatr.Commands.ClientProfiles;
using Frelance.Application.Repositories;
using MediatR;

namespace Frelance.Application.Mediatr.Handlers.ClientProfiles;

public class UpdateClientProfileCommandHandler : IRequestHandler<UpdateClientProfileCommand, Unit>
{
    private readonly IClientProfileRepository _clientProfileRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateClientProfileCommandHandler(IClientProfileRepository clientProfileRepository, IUnitOfWork unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(clientProfileRepository, nameof(clientProfileRepository));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        _clientProfileRepository = clientProfileRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UpdateClientProfileCommand request, CancellationToken cancellationToken)
    {
        await _clientProfileRepository.UpdateClientProfileAsync(request, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}