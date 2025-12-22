using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.Domain.Interfaces;
using Freelance.UserProfiles.Infrastructure.Persistance;
using Shared.Application.Mediator;
using Shared.Domain.Interfaces;

namespace Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Handlers;

public class UpdateFreelancerProfileAddressCommandHandler : IRequestHandler<UpdateFreelancerProfileAddressCommand, Unit>
{
    private readonly IFreelancerProfileRepository _freelancerProfileRepository;
    private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;

    public UpdateFreelancerProfileAddressCommandHandler(IFreelancerProfileRepository freelancerProfileRepository, IUnitOfWork<ApplicationDbContext> unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(freelancerProfileRepository, nameof(freelancerProfileRepository));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        _unitOfWork = unitOfWork;
        _freelancerProfileRepository = freelancerProfileRepository;
    }

    public async Task<Unit> Handle(UpdateFreelancerProfileAddressCommand request, CancellationToken cancellationToken)
    {
        var freelancerProfile =
            await _freelancerProfileRepository.GetByIdAsync(request.Id, cancellationToken, s => s.Skills, fL => fL.ForeignLanguages);
        if (freelancerProfile is null)
            throw new ProfileNotFoundException($"Profile with id {request.Id} not found");

        freelancerProfile.UpdateAddress(request.AddressDto.Street, request.AddressDto.City, request.AddressDto.State,
            request.AddressDto.ZipCode, request.AddressDto.Country, request.AddressDto.StreetNumber);
        _freelancerProfileRepository.Update(freelancerProfile);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}