using Freelance.UserProfiles.Application.Mappings;
using Freelance.UserProfiles.Domain.Entities;
using Freelance.UserProfiles.Domain.Interfaces;

namespace Freelance.UserProfiles.UnitTests.Application.Mappings;

public sealed class SkillResolutionTests
{
    [Fact]
    public async Task ResolveSkillsAsync_ReusesExistingSkills_AndAddsMissingOnes()
    {
        var ct = TestContext.Current.CancellationToken;
        var existing = Skill.Create("C#", "Backend");
        var repository = new Mock<ISkillRepository>();
        repository.Setup(r => r.FindAsync("C#", "Backend", ct)).ReturnsAsync(existing);

        var skills = await repository.Object.ResolveSkillsAsync(["C#", "Go"], ["Backend", "Cloud"], ct);

        skills.Should().HaveCount(2);
        skills[0].Should().BeSameAs(existing);
        skills[1].Should().BeEquivalentTo(new { ProgrammingLanguage = "Go", Area = "Cloud" });
        repository.Verify(r => r.AddAsync(It.Is<Skill>(s => s.ProgrammingLanguage == "Go"), ct), Times.Once);
        repository.Verify(r => r.AddAsync(existing, It.IsAny<CancellationToken>()), Times.Never);
    }
}
