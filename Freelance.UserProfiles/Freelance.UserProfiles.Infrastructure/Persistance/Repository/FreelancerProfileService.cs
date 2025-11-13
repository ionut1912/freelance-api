using Freelance.Shared.Domain.Interfaces;
using Freelance.UserProfiles.Domain.Entities;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Freelance.UserProfiles.Infrastructure.Persistance.Repository;

public class FreelancerProfileService:IFreelancerProfileRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;

    public FreelancerProfileService(ApplicationDbContext context, IUnitOfWork<ApplicationDbContext> unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        _context = context;
        _unitOfWork = unitOfWork;
    }
    
    public async Task CreateFreelancerProfileAsync(FreelancerProfile createFreelancerProfileRequest,
        CancellationToken cancellationToken)
    {
        await _context.FreelancerProfiles.AddAsync(createFreelancerProfileRequest, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateFreelancerProfileAsync(FreelancerProfile freelancerProfile, CancellationToken cancellationToken)
    {
        _context.FreelancerProfiles.Update(freelancerProfile);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateFreelancerProfileDetails(FreelancerProfile freelancerProfile,List<Skill> skills, CancellationToken cancellationToken)
    {
        var attachedSkills = await AttachExistingSkillsAsync(skills, cancellationToken);
        freelancerProfile.UpdateSkills(attachedSkills);
        _context.FreelancerProfiles.Update(freelancerProfile);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteFreelancerProfileAsync(FreelancerProfile freelancerProfile, CancellationToken cancellationToken)
    {
        _context.FreelancerProfiles.Remove(freelancerProfile);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<FreelancerProfile> GetFreelancerProfileByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var freelancerProfile=await _context.FreelancerProfiles
            .Include(f=>f.ForeignLanguages)
            .Include(f=>f.Skills)
            .FirstOrDefaultAsync(x=>x.Id == id, cancellationToken);
        return freelancerProfile==null ? throw new ProfileNotFoundException($"Profile with id {id} not found") : freelancerProfile;
    }

    public async Task<FreelancerProfile> GetLoggedInFreelancerProfileAsync(Guid accountId, CancellationToken cancellationToken)
    {
        var freelancerProfile=await _context.FreelancerProfiles
            .AsNoTracking()
            .Include(f=>f.ForeignLanguages)
            .Include(f=>f.Skills)
            .FirstOrDefaultAsync(x=>x.AccountId == accountId, cancellationToken);
        return freelancerProfile==null ? throw new ProfileNotFoundException($"Profile with accountId {accountId} not found") : freelancerProfile;
    }

    public async Task<List<FreelancerProfile>> GetFreelancerProfilesAsync(CancellationToken cancellationToken)
    {
        var freelancerProfiles = await _context.FreelancerProfiles
            .AsNoTracking()
            .Include(f=>f.ForeignLanguages)
            .Include(f=>f.Skills)
            .ToListAsync(cancellationToken);
        return freelancerProfiles;
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