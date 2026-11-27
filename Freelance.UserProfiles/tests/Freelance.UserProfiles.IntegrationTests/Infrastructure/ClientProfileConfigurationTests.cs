using System.Net;
using Freelance.UserProfiles.Domain.Entities;
using Freelance.UserProfiles.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Freelance.UserProfiles.IntegrationTests.Infrastructure;

public sealed class ClientProfileConfigurationTests(UserProfilesApiFactory factory)
{
    private readonly CancellationToken _ct = TestContext.Current.CancellationToken;

    [Fact]
    public async Task Configure_NewProfile_IsStoredUnverified()
    {
        var created = await UserProfilesApiFactory.CreateClientProfileAsync(factory.CreateClient("Client"), _ct);
        using var scope = factory.Services.CreateScope();

        var stored = await scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().ClientProfiles
            .AsNoTracking().SingleAsync(p => p.Id == created.Id, _ct);

        stored.IsVerified.Should().BeFalse();
    }

    [Fact(Skip = "Bug: no unique index on AccountId, so concurrent requests can create two profiles for one account. " +
                 "The fix needs a migration and a clean-up of existing duplicates.")]
    public async Task Configure_ConcurrentCreatesForOneAccount_StoreOneProfile()
    {
        var accountId = Guid.NewGuid();
        var client = factory.CreateClient("Client", accountId);

        var results = await Task.WhenAll(Enumerable.Range(0, 8).Select(async _ =>
        {
            try
            {
                await UserProfilesApiFactory.CreateClientProfileAsync(client, _ct);
                return HttpStatusCode.Created;
            }
            catch (HttpRequestException e)
            {
                return e.StatusCode!.Value;
            }
        }));

        results.Count(status => status == HttpStatusCode.Created).Should().Be(1);
    }
}
