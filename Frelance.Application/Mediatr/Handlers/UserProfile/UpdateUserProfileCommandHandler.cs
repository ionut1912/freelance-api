using Frelance.Application.Mediatr.Commands.UserProfile;
using Frelance.Application.Repositories;
using Frelance.Contracts.Enums;
using Frelance.Contracts.Requests.ClientProfile;
using Frelance.Contracts.Requests.FreelancerProfiles;
using MediatR;

namespace Frelance.Application.Mediatr.Handlers.UserProfile;

public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand>
{
    private readonly IClientProfileRepository _clientProfileRepository;
    private readonly IFreelancerProfileRepository _freelancerProfileRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserProfileCommandHandler(IFreelancerProfileRepository freelancerProfileRepository,
        IClientProfileRepository clientProfileRepository,
        IUnitOfWork unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(freelancerProfileRepository, nameof(freelancerProfileRepository));
        ArgumentNullException.ThrowIfNull(clientProfileRepository, nameof(clientProfileRepository));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        _freelancerProfileRepository = freelancerProfileRepository;
        _clientProfileRepository = clientProfileRepository;
        _unitOfWork = unitOfWork;

    }

    public async Task Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var repoTask = request.Role switch
        {
            Role.Freelancer when request.UpdateProfileRequest is UpdateFreelancerProfileRequest updateFreelancerProfileRequest =>
                _freelancerProfileRepository.UpdateFreelancerProfileAsync(request.Id, updateFreelancerProfileRequest, cancellationToken),
            Role.Client when request.UpdateProfileRequest is UpdateClientProfileRequest updateClientProfileRequest =>
                _clientProfileRepository.UpdateClientProfileAsync(request.Id, updateClientProfileRequest, cancellationToken),
            _ => throw new InvalidOperationException("Invalid request")
        };

        await repoTask;
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}