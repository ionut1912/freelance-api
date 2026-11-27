using Freelance.UserProfiles.Domain.Entities;
using Freelance.UserProfiles.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Shared.Domain.Interfaces;

namespace Freelance.UserProfiles.IntegrationTests.Infrastructure;

public sealed class ClientProfileServiceTests(UserProfilesApiFactory factory)
{
    private readonly CancellationToken _ct = TestContext.Current.CancellationToken;

    [Fact]
    public async Task GetLoggedInClientProfile_FindsByAccountOnly()
    {
        var profile = ClientProfile.Create(Guid.NewGuid(), "s", "c", "st", "z", "co", "n", "b", "i");
        using (var scope = factory.Services.CreateScope())
        {
            await scope.ServiceProvider.GetRequiredService<IClientProfileRepository>().AddAsync(profile, _ct);
            await scope.ServiceProvider.GetRequiredService<IUnitOfWork>().SaveChangesAsync(_ct);
        }

        using var read = factory.Services.CreateScope();
        var repository = read.ServiceProvider.GetRequiredService<IClientProfileRepository>();

        (await repository.GetLoggedInClientProfileAsync(profile.AccountId, _ct))!.Id.Should().Be(profile.Id);
        (await repository.GetLoggedInClientProfileAsync(profile.Id, _ct)).Should().BeNull();
    }
}
