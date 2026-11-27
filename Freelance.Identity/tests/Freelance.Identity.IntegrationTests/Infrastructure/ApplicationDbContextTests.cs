using Freelance.Identity.Domain.Entities;
using Freelance.Identity.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Freelance.Identity.IntegrationTests.Infrastructure;

public sealed class ApplicationDbContextTests(IdentityApiFactory factory)
{
    private readonly CancellationToken _ct = TestContext.Current.CancellationToken;

    [Fact]
    public async Task Accounts_IsQueryable_AfterMigrations()
    {
        using var scope = factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        (await context.Database.GetPendingMigrationsAsync(_ct)).Should().BeEmpty();
        (await context.Accounts.CountAsync(_ct)).Should().BeGreaterThanOrEqualTo(0);
    }

    [Fact]
    public void Model_MapsAccountWithUniqueUsernameAndOwnedRole()
    {
        using var scope = factory.Services.CreateScope();
        var entity = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Model.FindEntityType(typeof(Account))!;

        entity.GetIndexes().Should().ContainSingle(i => i.IsUnique && i.Properties.Single().Name == nameof(Account.Username));
        entity.FindNavigation(nameof(Account.Role))!.TargetEntityType.IsOwned().Should().BeTrue();
    }
}
