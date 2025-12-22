using Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Commands;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.Domain.Interfaces;
using Freelance.UserProfiles.Infrastructure.Persistance;
using Shared.Application.Mediator;
using Shared.Domain.Interfaces;
namespace Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Handlers;

public class DeleteClientProfileCommandHandler : IRequestHandler<DeleteClientProfileCommand, Unit>
{
    private readonly IClientProfileRepository _clientProfileRepository;
    private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;

    public DeleteClientProfileCommandHandler(IClientProfileRepository clientProfileRepository, IUnitOfWork<ApplicationDbContext> unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(clientProfileRepository, nameof(clientProfileRepository));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        _clientProfileRepository = clientProfileRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteClientProfileCommand request, CancellationToken cancellationToken)
    {
        var clientProfile = await _clientProfileRepository.GetByIdAsync(request.Id, cancellationToken) ?? throw new ProfileNotFoundException($"Client Profile with id {request.Id} does not exists");
        _clientProfileRepository.Delete(clientProfile);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}