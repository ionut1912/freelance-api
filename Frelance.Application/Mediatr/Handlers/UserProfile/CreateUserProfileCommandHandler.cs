using Frelance.Application.Mediatr.Commands.UserProfile;
using Frelance.Application.Repositories;
using Frelance.Contracts.Enums;
using Frelance.Contracts.Requests.ClientProfile;
using Frelance.Contracts.Requests.FreelancerProfiles;
using MediatR;

namespace Frelance.Application.Mediatr.Handlers.UserProfile;

public class CreateUserProfileCommandHandler:IRequestHandler<CreateUserProfileCommand,Unit>
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
    
    public async Task<Unit> Handle(CreateUserProfileCommand request, CancellationToken cancellationToken)
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
        return Unit.Value;
    }
}