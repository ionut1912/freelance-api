using Freelance.UserProfiles.Domain.Entities;
using Shared.Domain.Interfaces;

namespace Freelance.UserProfiles.Domain.Interfaces;

public interface ISkillRepository : IGenericRepository<Skill>
{
    /// <summary>Finds an existing (tracked) skill so it can be shared between profiles.</summary>
    Task<Skill?> FindAsync(string programmingLanguage, string area, CancellationToken cancellationToken);
}
