using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;
using Freelance.UserProfiles.Domain.Entities;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.Domain.Interfaces;
using Freelance.UserProfiles.Domain.ValueObjects;
using Freelance.UserProfiles.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Application.Mediator;
using Shared.Domain.Interfaces;
using System.Text.Json;

namespace Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Handlers;

public class UpdateFreelancerDetailsCommandHandler : IRequestHandler<UpdateFreelancerDetailsCommand, Unit>
{
    private readonly IFreelancerProfileRepository _freelancerProfileRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UpdateFreelancerDetailsCommandHandler> _logger;

    public UpdateFreelancerDetailsCommandHandler(IFreelancerProfileRepository freelancerProfileRepository, ApplicationDbContext context, IUnitOfWork unitOfWork,ILogger<UpdateFreelancerDetailsCommandHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(freelancerProfileRepository, nameof(freelancerProfileRepository));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        _unitOfWork = unitOfWork;
        _freelancerProfileRepository = freelancerProfileRepository;
        _context = context;
        _logger = logger;
    }

    public async Task<Unit> Handle(UpdateFreelancerDetailsCommand request, CancellationToken cancellationToken)
    {
        var freelancerProfile =await _freelancerProfileRepository.GetByIdAsync(request.Id, cancellationToken, s => s.Skills, fL => fL.ForeignLanguages);
        if (freelancerProfile is null)
        {
            _logger.LogError("Profile with id {ProfileId} not found", request.Id);
            throw new ProfileNotFoundException($"Profile with id {request.Id} not found");
        }

        var foreignLanguages = request.ForeignLanguages?
            .Select(language => FreelancerForeignLanguage.Create(language))
            .ToList();

        var skills = request.ProgrammingLanguages.Select((t, i) => Skill.Create(t, request.Areas[i])).ToList();

        freelancerProfile.UpdateLanguages(foreignLanguages!);
        freelancerProfile.UpdateFreelancerDetails(request.Experience, request.Amount, request.Currency,
            request.PortfolioUrl);

        var attachedSkills = await AttachExistingSkillsAsync(skills, cancellationToken);
        freelancerProfile.UpdateSkills(attachedSkills);
        _freelancerProfileRepository.Update(freelancerProfile);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Profile details for profile with id {ProfileId} updated successfully {newForeignLanguages}, {newSkills},{newExperience},{newAmount},{newCurrency},{newPortfolioUrl}", 
            request.Id,JsonSerializer.Serialize(freelancerProfile.ForeignLanguages, new JsonSerializerOptions { WriteIndented=true}),
            JsonSerializer.Serialize(freelancerProfile.Skills, new JsonSerializerOptions { WriteIndented = true }),
            freelancerProfile.Experience,freelancerProfile.Rate.Amount,freelancerProfile.Rate.Currency,freelancerProfile.PortfolioUrl);
        return Unit.Value;
    }

    private async Task<List<Skill>> AttachExistingSkillsAsync(
    IEnumerable<Skill> incomingSkills,
    CancellationToken cancellationToken)
    {
        var attachedSkills = new List<Skill>();

        foreach (var skill in incomingSkills)
        {
            var existing = await _context.Skill
                .FirstOrDefaultAsync(s =>
                        s.ProgrammingLanguage == skill.ProgrammingLanguage &&
                        s.Area == skill.Area,
                    cancellationToken);

            if (existing != null)
            {
                attachedSkills.Add(existing);
            }
            else
            {
                _context.Skill.Add(skill);
                attachedSkills.Add(skill);
            }
        }

        return attachedSkills;
    }
}