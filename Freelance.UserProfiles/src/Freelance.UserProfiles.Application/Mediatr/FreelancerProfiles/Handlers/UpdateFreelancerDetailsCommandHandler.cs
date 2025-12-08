using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;
using Freelance.UserProfiles.Domain.Entities;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.Domain.Interfaces;
using Freelance.UserProfiles.Domain.ValueObjects;
using Shared.Application.Mediator;

namespace Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Handlers;

public class UpdateFreelancerDetailsCommandHandler : IRequestHandler<UpdateFreelancerDetailsCommand, Unit>
{
    private readonly IFreelancerProfileRepository _freelancerProfileRepository;

    public UpdateFreelancerDetailsCommandHandler(IFreelancerProfileRepository freelancerProfileRepository)
    {
        ArgumentNullException.ThrowIfNull(freelancerProfileRepository, nameof(freelancerProfileRepository));
        _freelancerProfileRepository = freelancerProfileRepository;
    }

    public async Task<Unit> Handle(UpdateFreelancerDetailsCommand request, CancellationToken cancellationToken)
    {
        var freelancerProfile =
            await _freelancerProfileRepository.GetFreelancerProfileByIdAsync(request.Id, cancellationToken);
        if (freelancerProfile is null)
            throw new ProfileNotFoundException($"Profile with id {request.Id} not found");

        var foreignLanguages = request.ForeignLanguages?
            .Select(language => FreelancerForeignLanguage.Create(language))
            .ToList();

        var skills = request.ProgrammingLanguages.Select((t, i) => Skill.Create(t, request.Areas[i])).ToList();

        freelancerProfile.UpdateLanguages(foreignLanguages!);
        freelancerProfile.UpdateFreelancerDetails(request.Experience, request.Amount, request.Currency,
            request.PortfolioUrl);
        await _freelancerProfileRepository.UpdateFreelancerProfileDetails(freelancerProfile, skills, cancellationToken);
        return Unit.Value;
    }
}