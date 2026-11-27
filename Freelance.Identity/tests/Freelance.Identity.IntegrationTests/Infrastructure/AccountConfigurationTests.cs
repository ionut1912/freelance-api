using Freelance.Identity.Domain.Entities;
using Freelance.Identity.Domain.ValueObjects;
using Freelance.Identity.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Freelance.Identity.IntegrationTests.Infrastructure;

public sealed class AccountConfigurationTests(IdentityApiFactory factory)
{
    private readonly CancellationToken _ct = TestContext.Current.CancellationToken;

    private static Account NewAccount(string username) =>
        Account.Create($"{username}@test.io", "hash", "0700000000", username, Role.Client);

    private async Task SaveAsync(params Account[] accounts)
    {
        using var scope = factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        context.Accounts.AddRange(accounts);
        await context.SaveChangesAsync(_ct);
    }

    [Theory]
    [InlineData(nameof(Account.Email), 100)]
    [InlineData(nameof(Account.Password), 200)]
    [InlineData(nameof(Account.Username), 50)]
    public void Configure_LimitsColumnLength(string property, int maximum)
    {
        using var scope = factory.Services.CreateScope();
        var entity = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Model.FindEntityType(typeof(Account))!;

        entity.FindProperty(property)!.GetMaxLength().Should().Be(maximum);
    }

    [Fact]
    public async Task Configure_DuplicateUsername_IsRejectedByTheDatabase()
    {
        var username = IdentityApiFactory.NewUsername();
        await SaveAsync(NewAccount(username));

        var failure = (await FluentActions.Awaiting(() => SaveAsync(NewAccount(username)))
            .Should().ThrowAsync<DbUpdateException>()).Which;

        failure.InnerException.Should().BeOfType<PostgresException>()
            .Which.SqlState.Should().Be(PostgresErrorCodes.UniqueViolation);
    }

    [Fact]
    public async Task Configure_FailedBatch_RollsBackEveryRow()
    {
        var first = IdentityApiFactory.NewUsername();
        var duplicate = IdentityApiFactory.NewUsername();
        await SaveAsync(NewAccount(duplicate));

        await FluentActions.Awaiting(() => SaveAsync(NewAccount(first), NewAccount(duplicate)))
            .Should().ThrowAsync<DbUpdateException>();

        using var scope = factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        (await context.Accounts.AnyAsync(a => a.Username == first, _ct)).Should().BeFalse();
    }
}
