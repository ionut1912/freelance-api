using Freelance.Application.Mediatr.Commands.UserProfile;
using Freelance.Application.Repositories;
using Freelance.Contracts.Enums;
using Freelance.Contracts.Requests.ClientProfile;
using Freelance.Contracts.Requests.FreelancerProfiles;
using MediatR;

namespace Freelance.Application.Mediatr.Handlers.UserProfile;

public class CreateUserProfileCommandHandler : IRequestHandler<CreateUserProfileCommand>
{
    private readonly IClientProfileRepository _clientProfileRepository;
    private readonly IFreelancerProfileRepository _freelancerProfileRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserProfileCommandHandler(IClientProfileRepository clientProfileRepository,
        IFreelancerProfileRepository freelancerProfileRepository,
        IUnitOfWork unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(clientProfileRepository, nameof(clientProfileRepository));
        ArgumentNullException.ThrowIfNull(freelancerProfileRepository, nameof(freelancerProfileRepository));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        _clientProfileRepository = clientProfileRepository;
        _freelancerProfileRepository = freelancerProfileRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CreateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var repoTask = request.Role switch
        {
            Role.Freelancer when request.CreateProfileRequest is CreateFreelancerProfileRequest createProfileRequest =>
                _freelancerProfileRepository.CreateFreelancerProfileAsync(createProfileRequest, cancellationToken),
            Role.Client when request.CreateProfileRequest is CreateClientProfileRequest clientProfileRequest =>
                _clientProfileRepository.CreateClientProfileAsync(clientProfileRequest, cancellationToken),
            _ => throw new InvalidOperationException("Invalid request")
        };

        await repoTask;
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}