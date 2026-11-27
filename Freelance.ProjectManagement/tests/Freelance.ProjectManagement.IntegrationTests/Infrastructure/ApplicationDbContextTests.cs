using Freelance.ProjectManagement.Domain.Entities;
using Freelance.ProjectManagement.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Freelance.ProjectManagement.IntegrationTests.Infrastructure;

public sealed class ApplicationDbContextTests(ProjectManagementApiFactory factory)
{
    private readonly CancellationToken _ct = TestContext.Current.CancellationToken;

    [Fact]
    public async Task DbSets_AreQueryable_AfterMigrations()
    {
        using var scope = factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        (await context.Database.GetPendingMigrationsAsync(_ct)).Should().BeEmpty();
        (await context.Projects.CountAsync(_ct)).Should().BeGreaterThanOrEqualTo(0);
        (await context.ProjectTasks.CountAsync(_ct)).Should().BeGreaterThanOrEqualTo(0);
        (await context.TimeLogs.CountAsync(_ct)).Should().BeGreaterThanOrEqualTo(0);
    }

    [Fact]
    public void Model_UsesCompositeKeyForProjectTechnologies()
    {
        using var scope = factory.Services.CreateScope();
        var project = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Model.FindEntityType(typeof(Project))!;

        var technologies = project.FindNavigation(nameof(Project.Technologies))!.TargetEntityType;

        technologies.GetTableName().Should().Be("ProjectTechnologies");
        technologies.FindPrimaryKey()!.Properties.Select(p => p.Name).Should().Equal("ProjectId", "Technology");
    }
}
