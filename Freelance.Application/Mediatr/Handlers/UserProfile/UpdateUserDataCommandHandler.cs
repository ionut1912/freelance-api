using Freelance.Application.Mediatr.Commands.UserProfile;
using Freelance.Application.Repositories;
using Freelance.Contracts.Enums;
using MediatR;

namespace Freelance.Application.Mediatr.Handlers.UserProfile;

public class UpdateUserDataCommandHandler:IRequestHandler<UpdateUserDataCommand>
{
    private readonly IClientProfileRepository _clientProfileRepository;
    private readonly IFreelancerProfileRepository _freelancerProfileRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserDataCommandHandler(IClientProfileRepository clientProfileRepository,IFreelancerProfileRepository freelancerProfileRepository,IUnitOfWork unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(clientProfileRepository, nameof(clientProfileRepository));
        ArgumentNullException.ThrowIfNull(freelancerProfileRepository, nameof(freelancerProfileRepository));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        _clientProfileRepository = clientProfileRepository;
        _freelancerProfileRepository = freelancerProfileRepository;
        _unitOfWork = unitOfWork;
        
    }
    
    public async Task Handle(UpdateUserDataCommand request, CancellationToken cancellationToken)
    {
        var repoTask = request.Role switch
        {
            Role.Freelancer => _freelancerProfileRepository.UpdateUserDataAsync(request.UpdateUserRequest, cancellationToken),
            Role.Client => _clientProfileRepository.UpdateUserDataAsync(request.UpdateUserRequest, cancellationToken),
            _ => throw new InvalidOperationException("Invalid request")
        };

        await repoTask;
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}