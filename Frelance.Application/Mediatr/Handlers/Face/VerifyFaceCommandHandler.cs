using Frelance.Application.Mediatr.Commands.Face;
using Frelance.Application.Repositories;
using Frelance.Application.Repositories.ML;
using Frelance.Contracts.Dtos;
using Frelance.Contracts.Enums;
using Frelance.Contracts.Exceptions;
using Frelance.Contracts.Responses;
using MediatR;

namespace Frelance.Application.Mediatr.Handlers.Face;

public class VerifyFaceCommandHandler : IRequestHandler<VerifyFaceCommand, VerifyFaceResult>
{
    private readonly IClientProfileRepository _clientProfileRepository;
    private readonly IFreelancerProfileRepository _freelancerProfileRepository;
    private readonly IFaceComparisionService _faceComparisionService;
    private const double Threshold = 0.8;

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
        {
            throw new NotFoundException("User profile not found");
        }

        var similarity = await _faceComparisionService.CompareFacesAsync(userProfile.Image, request.Base64Face);
        return new VerifyFaceResult(similarity >= Threshold, similarity);
    }
}