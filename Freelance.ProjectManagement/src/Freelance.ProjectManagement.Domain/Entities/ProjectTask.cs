using Freelance.ProjectManagement.Domain.Exceptions;
using Freelance.ProjectManagement.Domain.ValueObjects;
using Shared.Domain.Common;

namespace Freelance.ProjectManagement.Domain.Entities;

public sealed class ProjectTask : Entity
{
    private readonly List<TimeLog> _timeLogs = [];

    private ProjectTask() // for EF Core
    {
    }

    private ProjectTask(Guid projectId, string title, string description, ProjectTaskStatus status,
        ProjectTaskPriority priority)
    {
        ArgumentNullException.ThrowIfNull(status);
        ArgumentNullException.ThrowIfNull(priority);

        ProjectId = projectId;
        Title = title;
        Description = description;
        Status = status;
        Priority = priority;
    }

    public Guid ProjectId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public IReadOnlyCollection<TimeLog> TimeLogs => _timeLogs.AsReadOnly();

    // NOTE: the misspelling is kept on purpose: it is the mapped column name in the existing migrations.
    public Guid FreelacerId { get; private set; }
    public ProjectTaskStatus Status { get; private set; } = ProjectTaskStatus.New;
    public ProjectTaskPriority Priority { get; private set; } = ProjectTaskPriority.Low;

    public static ProjectTask Create(Guid projectId, string title, string description, ProjectTaskStatus status,
        ProjectTaskPriority priority) =>
        new(projectId, title, description, status, priority);

    public void AddTimeLog(TimeLog timeLog)
    {
        ArgumentNullException.ThrowIfNull(timeLog);
        _timeLogs.Add(timeLog);
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(string title, string description, ProjectTaskStatus status, ProjectTaskPriority priority)
    {
        ArgumentNullException.ThrowIfNull(status);
        ArgumentNullException.ThrowIfNull(priority);

        if (!string.IsNullOrEmpty(title)) Title = title;
        if (!string.IsNullOrEmpty(description)) Description = description;
        Status = status;
        Priority = priority;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateTimeLog(Guid timeLogId, DateTime startTime, DateTime endTime)
    {
        var timeLog = _timeLogs.Find(x => x.Id == timeLogId)
                      ?? throw new TimeLogNotFoundException($"Time log with id {timeLogId} can not be found");

        timeLog.Update(startTime, endTime);
        UpdatedAt = DateTime.UtcNow;
    }

    public void AssignFreelancer(Guid freelancerId) => FreelacerId = freelancerId;
}
