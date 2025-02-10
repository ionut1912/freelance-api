using Frelance.Application.Mediatr.Commands.ClientProfiles;
using Frelance.Application.Repositories;
using MediatR;

namespace Frelance.Application.Mediatr.Handlers.ClientProfiles;

public class AddClientProfileCommandHandler : IRequestHandler<AddClientProfileCommand, Unit>
{
    private readonly IClientProfileRepository _clientProfileRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddClientProfileCommandHandler(IClientProfileRepository clientProfileRepository, IUnitOfWork unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(clientProfileRepository, nameof(clientProfileRepository));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        _clientProfileRepository = clientProfileRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Unit> Handle(AddClientProfileCommand request, CancellationToken cancellationToken)
    {
        await _clientProfileRepository.AddClientProfileAsync(request, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}