using Freelance.Application.Mediatr.Commands.UserProfile;
using Freelance.Application.Repositories;
using Freelance.Contracts.Enums;
using MediatR;

namespace Freelance.Application.Mediatr.Handlers.UserProfile;

public class UpdateImageCommandHandler:IRequestHandler<UpdateImageCommand>
{
    private readonly IClientProfileRepository _clientProfileRepository;
    private readonly IFreelancerProfileRepository _freelancerProfileRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateImageCommandHandler(IClientProfileRepository clientProfileRepository, IFreelancerProfileRepository freelancerProfileRepository,IUnitOfWork unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(clientProfileRepository, nameof(clientProfileRepository));
        ArgumentNullException.ThrowIfNull(freelancerProfileRepository, nameof(freelancerProfileRepository));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        _clientProfileRepository = clientProfileRepository;
        _freelancerProfileRepository = freelancerProfileRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task Handle(UpdateImageCommand request, CancellationToken cancellationToken)
    {
        var repoTask = request.Role switch
        {
            Role.Freelancer => _freelancerProfileRepository.UpdateImageAsync(request.UpdateImageRequest.Image, cancellationToken),
            Role.Client => _clientProfileRepository.UpdateImageAsync(request.UpdateImageRequest.Image, cancellationToken),
            _ => throw new InvalidOperationException("Invalid request")
        };

        await repoTask;
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}