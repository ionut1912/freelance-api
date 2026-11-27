using Freelance.UserProfiles.Domain.Entities;

namespace Freelance.UserProfiles.UnitTests.Domain.Entities;

public sealed class SkillTests
{
    [Fact]
    public void Create_SetsLanguageAndArea_WithIdentity()
    {
        var skill = Skill.Create("C#", "Backend");

        skill.Should().BeEquivalentTo(new { ProgrammingLanguage = "C#", Area = "Backend" });
        skill.Id.Should().NotBeEmpty();
        skill.Should().NotBe(Skill.Create("C#", "Backend"), "skills are entities, compared by identity");
    }
}
