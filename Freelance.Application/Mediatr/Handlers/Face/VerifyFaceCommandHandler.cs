using Freelance.Application.Mediatr.Commands.Face;
using Freelance.Application.Repositories;
using Freelance.Application.Repositories.ML;
using Freelance.Contracts.Dtos;
using Freelance.Contracts.Enums;
using Freelance.Contracts.Exceptions;
using Freelance.Contracts.Responses;
using MediatR;

namespace Freelance.Application.Mediatr.Handlers.Face;

public class VerifyFaceCommandHandler : IRequestHandler<VerifyFaceCommand, VerifyFaceResult>
{
    private readonly IClientProfileRepository _clientProfileRepository;
    private readonly IFaceComparisionService _faceComparisionService;
    private readonly IFreelancerProfileRepository _freelancerProfileRepository;

    public VerifyFaceCommandHandler(IClientProfileRepository clientProfileRepository,
        IFreelancerProfileRepository freelancerProfileRepository,
        IFaceComparisionService faceComparisionService)
    {
        ArgumentNullException.ThrowIfNull(clientProfileRepository, nameof(clientProfileRepository));
        ArgumentNullException.ThrowIfNull(freelancerProfileRepository, nameof(freelancerProfileRepository));
        ArgumentNullException.ThrowIfNull(faceComparisionService, nameof(faceComparisionService));
        _clientProfileRepository = clientProfileRepository;
        _freelancerProfileRepository = freelancerProfileRepository;
        _faceComparisionService = faceComparisionService;
    }

    public async Task<VerifyFaceResult> Handle(VerifyFaceCommand request, CancellationToken cancellationToken)
    {
        BaseProfileDto userProfile = request.Role switch
        {
            Role.Client => await _clientProfileRepository.GetLoggedInClientProfileAsync(cancellationToken),
            Role.Freelancer => await _freelancerProfileRepository.GetLoggedInFreelancerProfileAsync(cancellationToken),
            _ => throw new NotFoundException("Role not found")
        };

        if (userProfile is null || string.IsNullOrEmpty(userProfile.Image))
            throw new NotFoundException("User profile not found");

        var result = await _faceComparisionService.CompareFacesAsync(userProfile.Image, request.Base64Face);
        return new VerifyFaceResult(result.IsMatch, result.Confidence);
    }
}