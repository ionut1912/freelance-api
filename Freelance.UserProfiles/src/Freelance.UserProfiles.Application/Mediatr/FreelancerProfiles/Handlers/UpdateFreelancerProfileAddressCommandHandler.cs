using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Shared.Domain.Interfaces;

namespace Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Handlers;

public sealed class UpdateFreelancerProfileAddressCommandHandler(
    IFreelancerProfileRepository freelancerProfileRepository,
    IUnitOfWork unitOfWork,
    ILogger<UpdateFreelancerProfileAddressCommandHandler> logger)
{
    public async Task Handle(UpdateFreelancerProfileAddressCommand command, CancellationToken cancellationToken)
    {
        var profile = await freelancerProfileRepository.GetByIdWithTrackingAsync(command.Id, cancellationToken);
        // Someone else's profile is reported as missing, so ids cannot be probed.
        if (profile?.AccountId != command.AccountId)
            throw new ProfileNotFoundException($"Profile with id {command.Id} not found");

        var address = command.AddressDto;
        profile.UpdateAddress(address.Street, address.City, address.State, address.ZipCode, address.Country,
            address.StreetNumber);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Address of freelancer profile {ProfileId} was updated", command.Id);
    }
}
