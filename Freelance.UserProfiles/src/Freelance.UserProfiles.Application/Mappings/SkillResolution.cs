using Freelance.UserProfiles.Domain.Entities;
using Freelance.UserProfiles.Domain.Interfaces;

namespace Freelance.UserProfiles.Application.Mappings;

public static class SkillResolution
{
    extension(ISkillRepository skillRepository)
    {
        /// <summary>
        /// Pairs programming languages with areas and reuses existing <see cref="Skill"/> rows instead of
        /// inserting duplicates (skills are shared between profiles through the FreelancerSkills join table).
        /// </summary>
        public async Task<List<Skill>> ResolveSkillsAsync(IReadOnlyList<string> programmingLanguages,
            IReadOnlyList<string> areas, CancellationToken cancellationToken)
        {
            var skills = new List<Skill>(programmingLanguages.Count);
            foreach (var (language, area) in programmingLanguages.Zip(areas).Distinct())
            {
                var skill = await skillRepository.FindAsync(language, area, cancellationToken);
                if (skill is null)
                {
                    skill = Skill.Create(language, area);
                    await skillRepository.AddAsync(skill, cancellationToken);
                }

                skills.Add(skill);
            }

            return skills;
        }
    }
}
