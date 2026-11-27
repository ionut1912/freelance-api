using Freelance.Identity.Domain.Entities;
using Freelance.Identity.Domain.interfaces;
using Freelance.Identity.Domain.ValueObjects;
using Microsoft.Extensions.DependencyInjection;
using Shared.Domain.Interfaces;

namespace Freelance.Identity.IntegrationTests.Infrastructure;

public sealed class AccountRepositoryTests(IdentityApiFactory factory)
{
    private readonly CancellationToken _ct = TestContext.Current.CancellationToken;

    [Fact]
    public async Task AddedAccount_CanBeFoundByUsernameAndId_WithOwnedRole()
    {
        var username = IdentityApiFactory.NewUsername();
        var account = Account.Create($"{username}@test.io", "hash", "0700000000", username, Role.Freelancer);

        using (var scope = factory.Services.CreateScope())
        {
            await scope.ServiceProvider.GetRequiredService<IAccountRepository>().AddAsync(account, _ct);
            await scope.ServiceProvider.GetRequiredService<IUnitOfWork>().SaveChangesAsync(_ct);
        }

        using var readScope = factory.Services.CreateScope();
        var repository = readScope.ServiceProvider.GetRequiredService<IAccountRepository>();
        var byName = await repository.GetAccountByUsernameAsync(username, _ct);
        var byId = await repository.GetByIdAsync(account.Id, _ct);

        byName.Should().NotBeNull();
        byName!.Role.Should().Be(Role.Freelancer);
        byId!.Username.Should().Be(username);
    }

    [Fact]
    public async Task GetAccountByUsername_Unknown_ReturnsNull()
    {
        using var scope = factory.Services.CreateScope();

        var account = await scope.ServiceProvider.GetRequiredService<IAccountRepository>()
            .GetAccountByUsernameAsync("does-not-exist", _ct);

        account.Should().BeNull();
    }
}
