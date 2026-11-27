using Freelance.ProjectManagement.Domain.Entities;
using Freelance.ProjectManagement.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Freelance.ProjectManagement.IntegrationTests.Infrastructure;

public sealed class ProjectTaskConfigurationTests(ProjectManagementApiFactory factory)
{
    [Theory]
    [InlineData(nameof(ProjectTask.Title), 100)]
    [InlineData(nameof(ProjectTask.Description), 500)]
    public void Configure_LimitsColumnLength(string property, int maximum)
    {
        using var scope = factory.Services.CreateScope();
        var entity = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Model.FindEntityType(typeof(ProjectTask))!;

        entity.FindProperty(property)!.GetMaxLength().Should().Be(maximum);
    }

    [Theory]
    [InlineData(nameof(ProjectTask.Status))]
    [InlineData(nameof(ProjectTask.Priority))]
    public void Configure_StoresValueObjectsAsOwnedColumns(string navigation)
    {
        using var scope = factory.Services.CreateScope();
        var entity = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Model.FindEntityType(typeof(ProjectTask))!;

        var owned = entity.FindNavigation(navigation)!.TargetEntityType;

        owned.IsOwned().Should().BeTrue();
        owned.GetTableName().Should().Be(entity.GetTableName());
    }
}
