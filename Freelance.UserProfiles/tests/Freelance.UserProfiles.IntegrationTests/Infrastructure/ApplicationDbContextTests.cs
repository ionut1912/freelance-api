using Freelance.UserProfiles.Domain.Entities;
using Freelance.UserProfiles.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Freelance.UserProfiles.IntegrationTests.Infrastructure;

public sealed class ApplicationDbContextTests(UserProfilesApiFactory factory)
{
    private readonly CancellationToken _ct = TestContext.Current.CancellationToken;

    [Fact]
    public async Task DbSets_AreQueryable_AfterMigrations()
    {
        using var scope = factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        (await context.Database.GetPendingMigrationsAsync(_ct)).Should().BeEmpty();
        (await context.ClientProfiles.CountAsync(_ct)).Should().BeGreaterThanOrEqualTo(0);
        (await context.FreelancerProfiles.CountAsync(_ct)).Should().BeGreaterThanOrEqualTo(0);
        (await context.Skill.CountAsync(_ct)).Should().BeGreaterThanOrEqualTo(0);
    }

    [Fact]
    public void Model_AllowsSeveralLanguagesPerFreelancer()
    {
        using var scope = factory.Services.CreateScope();
        var freelancer = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Model.FindEntityType(typeof(FreelancerProfile))!;

        var languages = freelancer.FindNavigation(nameof(FreelancerProfile.ForeignLanguages))!.TargetEntityType;

        languages.GetTableName().Should().Be("FreelancerForeignLanguages");
        languages.FindPrimaryKey()!.Properties.Select(p => p.Name).Should().Equal("FreelancerProfileId", "Language");
    }
}
