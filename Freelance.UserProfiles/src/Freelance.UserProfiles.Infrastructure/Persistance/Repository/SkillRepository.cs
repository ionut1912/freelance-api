using Freelance.UserProfiles.Domain.Entities;
using Freelance.UserProfiles.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Infra.Services;

namespace Freelance.UserProfiles.Infrastructure.Persistance.Repository;

public sealed class SkillRepository(DbSet<Skill> dbSet) : GenericRepository<Skill>(dbSet), ISkillRepository
{
    private readonly DbSet<Skill> _skills = dbSet;

    // Tracked on purpose: the returned skill is attached to a profile in the same unit of work.
    public Task<Skill?> FindAsync(string programmingLanguage, string area, CancellationToken cancellationToken) =>
        _skills.FirstOrDefaultAsync(s => s.ProgrammingLanguage == programmingLanguage && s.Area == area,
            cancellationToken);
}
