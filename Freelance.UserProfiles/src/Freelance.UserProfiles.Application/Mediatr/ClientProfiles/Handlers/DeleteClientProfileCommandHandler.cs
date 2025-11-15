using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.Domain.Interfaces;
using Freelancer.UserProfiles.Application.Mediatr.ClientProfiles.Commands;
using MediatR;

namespace Freelancer.UserProfiles.Application.Mediatr.ClientProfiles.Handlers;

public class DeleteClientProfileCommandHandler : IRequestHandler<DeleteClientProfileCommand, Unit>
{
    private readonly IClientProfileRepository _clientProfileRepository;

    public DeleteClientProfileCommandHandler(IClientProfileRepository clientProfileRepository)
    {
        ArgumentNullException.ThrowIfNull(clientProfileRepository, nameof(clientProfileRepository));
        _clientProfileRepository = clientProfileRepository;
    }

    public async Task<Unit> Handle(DeleteClientProfileCommand request, CancellationToken cancellationToken)
    {
        var clientProfile = await _clientProfileRepository.GetClientProfileByIdAsync(request.Id, cancellationToken);
        if (clientProfile == null)
            throw new ProfileNotFoundException($"Client Profile with id {request.Id} does not exists");

        await _clientProfileRepository.DeleteClientProfileAsync(clientProfile, cancellationToken);
        return Unit.Value;
    }
}