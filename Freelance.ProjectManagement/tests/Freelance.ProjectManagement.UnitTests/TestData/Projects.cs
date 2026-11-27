using Freelance.ProjectManagement.Domain.Entities;
using Freelance.ProjectManagement.Domain.ValueObjects;

namespace Freelance.ProjectManagement.UnitTests.TestData;

internal static class Projects
{
    public static readonly DateTime Deadline = new(2030, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public static Project Project() => global::Freelance.ProjectManagement.Domain.Entities.Project.Create("Site", "Build site", Deadline, 1000, "eur");

    public static readonly Guid ClientId = Guid.NewGuid();
    public static readonly Guid FreelancerId = Guid.NewGuid();

    public static Project AssignedProject()
    {
        var project = Project();
        project.AssignClient(ClientId);
        project.AssignFreelancer(FreelancerId);
        return project;
    }

    public static ProjectTask Task(Guid? projectId = null) =>
        ProjectTask.Create(projectId ?? Guid.NewGuid(), "API", "Build API", ProjectTaskStatus.New, ProjectTaskPriority.High);

    public static TimeLog TimeLog(Guid? taskId = null) =>
        global::Freelance.ProjectManagement.Domain.Entities.TimeLog.Create(taskId ?? Guid.NewGuid(), Deadline, Deadline.AddHours(3));
}
