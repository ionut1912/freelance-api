using Freelance.ProjectManagement.Domain.Entities;
using Freelance.ProjectManagement.Domain.ValueObjects;
using Freelance.ProjectManagement.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Freelance.ProjectManagement.IntegrationTests.Infrastructure;

public sealed class ProjectConfigurationTests(ProjectManagementApiFactory factory)
{
    private readonly CancellationToken _ct = TestContext.Current.CancellationToken;

    private static Project NewProject(string title = "Site") =>
        Project.Create(title, "Build", new DateTime(2030, 1, 1, 0, 0, 0, DateTimeKind.Utc), 1234.5678m, "eur");

    private async Task<T> WithContextAsync<T>(Func<ApplicationDbContext, Task<T>> action)
    {
        using var scope = factory.Services.CreateScope();
        return await action(scope.ServiceProvider.GetRequiredService<ApplicationDbContext>());
    }

    private Task<int> SaveAsync(Project project) => WithContextAsync(context =>
    {
        context.Projects.Add(project);
        return context.SaveChangesAsync(_ct);
    });

    [Fact]
    public async Task Configure_RoundTripsBudgetPrecisionAndDeadline()
    {
        var project = NewProject();
        await SaveAsync(project);

        var loaded = await WithContextAsync(c => c.Projects.AsNoTracking().SingleAsync(p => p.Id == project.Id, _ct));

        loaded.Budget.Amount.Should().Be(1234.5678m);
        loaded.Budget.Currency.Should().Be("EUR");
        loaded.Deadline.Should().Be(project.Deadline);
    }

    [Fact]
    public async Task Configure_TitleLongerThanColumn_IsRejectedByTheDatabase() =>
        await FluentActions.Awaiting(() => SaveAsync(NewProject(new string('a', 51))))
            .Should().ThrowAsync<DbUpdateException>();

    [Fact]
    public async Task Configure_DeletingAProject_CascadesToTasksAndTimeLogs()
    {
        var project = NewProject();
        project.AddTechnologies([ProjectTechnology.Create("dotnet")]);
        var task = ProjectTask.Create(project.Id, "API", "Build", ProjectTaskStatus.New, ProjectTaskPriority.Low);
        var log = TimeLog.Create(task.Id, project.Deadline, project.Deadline.AddHours(1));
        task.AddTimeLog(log);
        project.AddTask(task);
        await SaveAsync(project);

        await WithContextAsync(async context =>
        {
            context.Projects.Remove(await context.Projects.SingleAsync(p => p.Id == project.Id, _ct));
            return await context.SaveChangesAsync(_ct);
        });

        (await WithContextAsync(c => c.ProjectTasks.AnyAsync(t => t.Id == task.Id, _ct))).Should().BeFalse();
        (await WithContextAsync(c => c.TimeLogs.AnyAsync(t => t.Id == log.Id, _ct))).Should().BeFalse();
    }
}
