using System.Net;
using Freelance.UserProfiles.Domain.Entities;
using Freelance.UserProfiles.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Freelance.UserProfiles.IntegrationTests.Infrastructure;

public sealed class FreelanceProfileConfigurationTests(UserProfilesApiFactory factory)
{
    private readonly CancellationToken _ct = TestContext.Current.CancellationToken;

    [Fact]
    public async Task Configure_DeletingAProfile_KeepsTheSharedSkills()
    {
        var freelancer = factory.CreateClient("Freelancer");
        var created = await UserProfilesApiFactory.CreateFreelancerProfileAsync(freelancer, _ct);

        (await freelancer.DeleteAsync($"/freelancer-profiles/{created.Id}", _ct)).StatusCode
            .Should().Be(HttpStatusCode.NoContent);

        using var scope = factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        (await context.FreelancerProfiles.AnyAsync(p => p.Id == created.Id, _ct)).Should().BeFalse();
        (await context.Set<Skill>().AnyAsync(s => s.ProgrammingLanguage == "C#", _ct)).Should().BeTrue();
    }

    [Fact]
    public void Configure_LimitsRateCurrencyToThreeCharacters()
    {
        using var scope = factory.Services.CreateScope();
        var rate = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Model
            .FindEntityType(typeof(FreelancerProfile))!.FindNavigation(nameof(FreelancerProfile.Rate))!.TargetEntityType;

        rate.FindProperty("Currency")!.GetMaxLength().Should().Be(3);
    }
}
