using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;
using Freelance.UserProfiles.Domain.Entities;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.Domain.Interfaces;
using Freelance.UserProfiles.Domain.ValueObjects;
using Freelance.UserProfiles.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Mediator;
using Shared.Domain.Interfaces;

namespace Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Handlers;

public class UpdateFreelancerDetailsCommandHandler : IRequestHandler<UpdateFreelancerDetailsCommand, Unit>
{
    private readonly IFreelancerProfileRepository _freelancerProfileRepository;
    private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;
    private readonly ApplicationDbContext _context;

    public UpdateFreelancerDetailsCommandHandler(IFreelancerProfileRepository freelancerProfileRepository, ApplicationDbContext context, IUnitOfWork<ApplicationDbContext> unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(freelancerProfileRepository, nameof(freelancerProfileRepository));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        _unitOfWork = unitOfWork;
        _freelancerProfileRepository = freelancerProfileRepository;
        _context = context;
    }

    public async Task<Unit> Handle(UpdateFreelancerDetailsCommand request, CancellationToken cancellationToken)
    {
        var freelancerProfile =
            await _freelancerProfileRepository.GetByIdAsync(request.Id, cancellationToken, s => s.Skills, fL => fL.ForeignLanguages);
        if (freelancerProfile is null)
            throw new ProfileNotFoundException($"Profile with id {request.Id} not found");

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