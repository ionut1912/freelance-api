using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.Domain.Interfaces;
using Freelancer.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;
using MediatR;

namespace Freelancer.UserProfiles.Application.Mediatr.FreelancerProfiles.Handlers;

public class UpdateFreelancerProfileAddressCommandHandler : IRequestHandler<UpdateFreelancerProfileAddressCommand, Unit>
{
    private readonly IFreelancerProfileRepository _freelancerProfileRepository;

    public UpdateFreelancerProfileAddressCommandHandler(IFreelancerProfileRepository freelancerProfileRepository)
    {
        ArgumentNullException.ThrowIfNull(freelancerProfileRepository);
        _freelancerProfileRepository = freelancerProfileRepository;
    }

    public async Task<Unit> Handle(UpdateFreelancerProfileAddressCommand request, CancellationToken cancellationToken)
    {
        var freelancerProfile =
            await _freelancerProfileRepository.GetFreelancerProfileByIdAsync(request.Id, cancellationToken);
        if (freelancerProfile is null)
            throw new ProfileNotFoundException($"Profile with id {request.Id} not found");

        freelancerProfile.UpdateAddress(request.AddressDto.Street, request.AddressDto.City, request.AddressDto.State,
            request.AddressDto.ZipCode, request.AddressDto.Country, request.AddressDto.StreetNumber);
        await _freelancerProfileRepository.UpdateFreelancerProfileAsync(freelancerProfile, cancellationToken);
        return Unit.Value;
    }
}