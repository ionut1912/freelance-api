using Freelance.UserProfiles.Domain.Entities;
using Freelance.UserProfiles.Domain.Interfaces;
using Freelance.UserProfiles.Domain.ValueObjects;
using Microsoft.Extensions.DependencyInjection;
using Shared.Domain.Interfaces;

namespace Freelance.UserProfiles.IntegrationTests.Infrastructure;

/// <summary>Covers ClientProfileService, FreelancerProfileService and SkillRepository against PostgreSQL.</summary>
public sealed class RepositoryTests(UserProfilesApiFactory factory)
{
    private readonly CancellationToken _ct = TestContext.Current.CancellationToken;

    private async Task SaveAsync(Func<IServiceProvider, Task> action)
    {
        using var scope = factory.Services.CreateScope();
        await action(scope.ServiceProvider);
        await scope.ServiceProvider.GetRequiredService<IUnitOfWork>().SaveChangesAsync(_ct);
    }

    [Fact]
    public async Task FreelancerProfile_RoundTripsLanguagesAndSharedSkills()
    {
        var skill = Skill.Create($"Lang{Guid.NewGuid():N}", "Backend");
        var first = FreelancerProfile.Create(Guid.NewGuid(), "s", "c", "st", "z", "co", "n", "b", "i", "e", 1, "EUR", 0, "p");
        var second = FreelancerProfile.Create(Guid.NewGuid(), "s", "c", "st", "z", "co", "n", "b", "i", "e", 1, "EUR", 0, "p");
        first.AddLanguages([FreelancerForeignLanguage.Create("English"), FreelancerForeignLanguage.Create("French")]);
        first.AddSkills([skill]);

        await SaveAsync(async sp =>
        {
            await sp.GetRequiredService<ISkillRepository>().AddAsync(skill, _ct);
            await sp.GetRequiredService<IFreelancerProfileRepository>().AddAsync(first, _ct);
        });
        await SaveAsync(async sp =>
        {
            var shared = await sp.GetRequiredService<ISkillRepository>().FindAsync(skill.ProgrammingLanguage, "Backend", _ct);
            second.AddSkills([shared!]);
            await sp.GetRequiredService<IFreelancerProfileRepository>().AddAsync(second, _ct);
        });

        using var scope = factory.Services.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IFreelancerProfileRepository>();
        var loaded = await repository.GetLoggedInFreelancerProfileAsync(first.AccountId, _ct);
        var other = await repository.GetLoggedInFreelancerProfileAsync(second.AccountId, _ct);

        loaded!.ForeignLanguages.Select(l => l.Language).Should().BeEquivalentTo("English", "French");
        other!.Skills.Should().ContainSingle().Which.Id.Should().Be(skill.Id, "skills are shared, not duplicated");
        (await repository.GetLoggedInFreelancerProfileAsync(Guid.NewGuid(), _ct)).Should().BeNull();
        (await repository.GetByIdWithTrackingAsync(first.Id, _ct, null!)).Should().NotBeNull();
    }

    [Fact]
    public async Task ClientProfile_CanBeFoundByAccount()
    {
        var profile = ClientProfile.Create(Guid.NewGuid(), "s", "c", "st", "z", "co", "n", "b", "i");
        await SaveAsync(sp => sp.GetRequiredService<IClientProfileRepository>().AddAsync(profile, _ct));

        using var scope = factory.Services.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IClientProfileRepository>();

        (await repository.GetLoggedInClientProfileAsync(profile.AccountId, _ct))!.Id.Should().Be(profile.Id);
        (await repository.GetLoggedInClientProfileAsync(Guid.NewGuid(), _ct)).Should().BeNull();
    }

    [Fact]
    public async Task SkillRepository_FindAsync_Unknown_ReturnsNull()
    {
        using var scope = factory.Services.CreateScope();

        (await scope.ServiceProvider.GetRequiredService<ISkillRepository>().FindAsync("none", "none", _ct)).Should().BeNull();
    }
}
