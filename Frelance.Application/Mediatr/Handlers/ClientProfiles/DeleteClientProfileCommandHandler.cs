using Frelance.Application.Mediatr.Commands.ClientProfiles;
using Frelance.Application.Repositories;
using MediatR;

namespace Frelance.Application.Mediatr.Handlers.ClientProfiles;

public class DeleteClientProfileCommandHandler:IRequestHandler<DeleteClientProfileCommand, Unit>
{
    private readonly IClientProfileRepository _clientProfileRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteClientProfileCommandHandler(IClientProfileRepository clientProfileRepository, IUnitOfWork unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(clientProfileRepository, nameof(clientProfileRepository));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        _clientProfileRepository = clientProfileRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Unit> Handle(DeleteClientProfileCommand request, CancellationToken cancellationToken)
    {
        await _clientProfileRepository.DeleteClientProfileAsync(request, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}