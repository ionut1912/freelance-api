using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;
using Freelance.UserProfiles.Domain.Entities;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.Domain.Interfaces;
using Freelance.UserProfiles.Infrastructure.Persistance;
using Microsoft.Extensions.Logging;
using Shared.Application.Mediator;
using Shared.Domain.Interfaces;
using System.Text.Json;

namespace Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Handlers;

public class UpdateFreelancerProfileAddressCommandHandler : IRequestHandler<UpdateFreelancerProfileAddressCommand, Unit>
{
    private readonly IFreelancerProfileRepository _freelancerProfileRepository;
    private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;
    private readonly ILogger<UpdateFreelancerProfileAddressCommandHandler> _logger;

    public UpdateFreelancerProfileAddressCommandHandler(IFreelancerProfileRepository freelancerProfileRepository, IUnitOfWork<ApplicationDbContext> unitOfWork,ILogger<UpdateFreelancerProfileAddressCommandHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(freelancerProfileRepository, nameof(freelancerProfileRepository));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        ArgumentNullException.ThrowIfNull(logger,nameof(logger));
        _unitOfWork = unitOfWork;
        _freelancerProfileRepository = freelancerProfileRepository;
        _logger = logger;
    }

    public async Task<Unit> Handle(UpdateFreelancerProfileAddressCommand request, CancellationToken cancellationToken)
    {
        var freelancerProfile =
            await _freelancerProfileRepository.GetByIdAsync(request.Id, cancellationToken, s => s.Skills, fL => fL.ForeignLanguages);
        if (freelancerProfile is null)
        {
            _logger.LogError("Profile with id {ProfileId} not found", request.Id);
            throw new ProfileNotFoundException($"Profile with id {request.Id} not found");
        }

        freelancerProfile.UpdateAddress(request.AddressDto.Street, request.AddressDto.City, request.AddressDto.State,
            request.AddressDto.ZipCode, request.AddressDto.Country, request.AddressDto.StreetNumber);
        _freelancerProfileRepository.Update(freelancerProfile);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Profile with id {ProfileId} updated successfully, {newAddress}", request.Id, JsonSerializer.Serialize(freelancerProfile.Address, new JsonSerializerOptions { WriteIndented = true }));
        return Unit.Value;
    }
}