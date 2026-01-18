using Freelance.ProjectManagement.Domain.Exceptions;
using Freelance.ProjectManagement.Domain.ValueObjects;
using Shared.Domain.Common;

namespace Freelance.ProjectManagement.Domain.Entities;

public class ProjectTask : Entity
{
    private ProjectTask() //for EF core
    {

    }

    private ProjectTask(Guid projectId, string title, string description, ProjectTaskStatus status, ProjectTaskPriority priority)
    {
        ProjectId = projectId;
        Title = title;
        Description = description;
        Status = status;
        Priority = priority;
    }

    public Guid ProjectId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    private readonly List<TimeLog> _timeLogs = [];
    public IReadOnlyCollection<TimeLog> TimeLogs => _timeLogs.AsReadOnly();
    public Guid FreelacerId { get; private set; }
    public ProjectTaskStatus Status { get; private set; } = ProjectTaskStatus.New;
    public ProjectTaskPriority Priority { get; private set; } = ProjectTaskPriority.Low;

    public static ProjectTask Create(Guid projectId, string title, string description, ProjectTaskStatus status, ProjectTaskPriority priority)
    {
        return new ProjectTask(projectId, title, description, status, priority);
    }

    public void AddTimeLog(TimeLog timeLog)
    {
        ArgumentNullException.ThrowIfNull(timeLog);
        _timeLogs.Add(timeLog);
    }

    public void Update(string title, string description, ProjectTaskStatus status, ProjectTaskPriority priority)
    {

        if (!string.IsNullOrEmpty(title))
        {
            Title= title;
        }
        if (!string.IsNullOrEmpty(description))
        {
            Description= description;
        }
        Status = status;
        Priority = priority;
    }

    public void UpdateTimelog(TimeLog timeLog)
    {
        ArgumentNullException.ThrowIfNull(timeLog);
        var existingTimelog= _timeLogs.FirstOrDefault(x=>x.Id == timeLog.Id);
        if (existingTimelog == null) throw new TimeLogNotFoundException($"Time log with id {timeLog.Id} can not be found");
        existingTimelog.Update(timeLog.StartTime, timeLog.EndTime);
    }

    public void AssignFreelancer(Guid freelancerId)
    {
        FreelacerId = freelancerId;
    }
}
