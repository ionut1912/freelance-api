using Frelance.Application.Mediatr.Commands.Face;
using Frelance.Application.Repositories;
using Frelance.Application.Repositories.ML;
using Frelance.Contracts.Dtos;
using Frelance.Contracts.Enums;
using Frelance.Contracts.Exceptions;
using Frelance.Contracts.Responses;
using MediatR;

namespace Frelance.Application.Mediatr.Handlers.Face;

/// <summary>
///     Handles face verification by comparing a stored profile image with a provided image.
/// </summary>
public class VerifyFaceCommandHandler : IRequestHandler<VerifyFaceCommand, VerifyFaceResult>
{
    private readonly IClientProfileRepository _clientProfileRepository;
    private readonly IFaceComparisionService _faceComparisionService;
    private readonly IFreelancerProfileRepository _freelancerProfileRepository;

    /// <summary>
    ///     Initializes a new instance of the <see cref="VerifyFaceCommandHandler" /> class.
    /// </summary>
    /// <param name="clientProfileRepository">The client profile repository.</param>
    /// <param name="freelancerProfileRepository">The freelancer profile repository.</param>
    /// <param name="faceComparisionService">The face comparison service.</param>
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

    /// <summary>
    ///     Processes the verification command by comparing the stored user profile image with the provided image.
    /// </summary>
    /// <param name="request">The verification command request.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A <see cref="VerifyFaceResult" /> containing the matching decision and confidence score.</returns>
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