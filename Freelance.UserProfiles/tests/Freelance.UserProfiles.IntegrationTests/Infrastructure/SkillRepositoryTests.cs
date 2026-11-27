using Freelance.UserProfiles.Domain.Entities;
using Freelance.UserProfiles.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Shared.Domain.Interfaces;

namespace Freelance.UserProfiles.IntegrationTests.Infrastructure;

public sealed class SkillRepositoryTests(UserProfilesApiFactory factory)
{
    private readonly CancellationToken _ct = TestContext.Current.CancellationToken;

    [Fact]
    public async Task FindAsync_MatchesLanguageAndAreaTogether()
    {
        var language = $"Lang{Guid.NewGuid():N}";
        using (var scope = factory.Services.CreateScope())
        {
            await scope.ServiceProvider.GetRequiredService<ISkillRepository>().AddAsync(Skill.Create(language, "Backend"), _ct);
            await scope.ServiceProvider.GetRequiredService<IUnitOfWork>().SaveChangesAsync(_ct);
        }

        using var read = factory.Services.CreateScope();
        var repository = read.ServiceProvider.GetRequiredService<ISkillRepository>();

        (await repository.FindAsync(language, "Backend", _ct)).Should().NotBeNull();
        (await repository.FindAsync(language, "Frontend", _ct)).Should().BeNull();
    }
}
