using Freelance.ProjectManagement.Domain.Exceptions;
using Freelance.ProjectManagement.Domain.ValueObjects;
using Shared.Domain.Common;

namespace Freelance.ProjectManagement.Domain.Entities;

public sealed class Project : Entity
{
    private readonly List<ProjectTechnology> _technologies = [];
    private readonly List<ProjectTask> _tasks = [];

    private Project() // for EF Core
    {
    }

    private Project(string title, string description, DateTime deadline, decimal amount, string currency)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentException.ThrowIfNullOrWhiteSpace(description);

        Title = title;
        Description = description;
        Deadline = UtcDateTime.From(deadline);
        Budget = Money.Create(amount, currency);
    }

    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public DateTime Deadline { get; private set; }
    public Guid ClientId { get; private set; }
    public Guid FreelancerId { get; private set; }
    public IReadOnlyCollection<ProjectTechnology> Technologies => _technologies.AsReadOnly();
    public IReadOnlyCollection<ProjectTask> Tasks => _tasks.AsReadOnly();
    public Money Budget { get; private set; } = Money.Create(0, "USD");

    public static Project Create(string title, string description, DateTime deadline, decimal amount, string currency) =>
        new(title, description, deadline, amount, currency);

    public void AddTechnologies(IEnumerable<ProjectTechnology> technologies)
    {
        ArgumentNullException.ThrowIfNull(technologies);
        foreach (var technology in technologies)
        {
            if (!_technologies.Exists(t => t.Technology == technology.Technology))
                _technologies.Add(technology);
        }
    }

    public void AddTask(ProjectTask task)
    {
        ArgumentNullException.ThrowIfNull(task);
        _tasks.Add(task);
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(string title, string description, DateTime deadline, decimal amount, string currency)
    {
        if (!string.IsNullOrEmpty(title)) Title = title;
        if (!string.IsNullOrEmpty(description)) Description = description;

        Deadline = UtcDateTime.From(deadline);
        Budget = Money.Create(amount, currency);
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateTask(Guid taskId, string title, string description, ProjectTaskStatus status,
        ProjectTaskPriority priority)
    {
        var task = _tasks.Find(x => x.Id == taskId)
                   ?? throw new ProjectTaskNotFoundException($"Project task with id {taskId} was not found");

        task.Update(title, description, status, priority);
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateTechnologies(IEnumerable<ProjectTechnology> technologies)
    {
        ArgumentNullException.ThrowIfNull(technologies);

        // Diff instead of Clear(): unchanged technologies keep their row (the key is ProjectId + Technology).
        var desired = technologies.Distinct().ToList();
        _technologies.RemoveAll(existing => !desired.Contains(existing));
        AddTechnologies(desired);
        UpdatedAt = DateTime.UtcNow;
    }

    public void AssignClient(Guid clientId) => ClientId = clientId;

    public void AssignFreelancer(Guid freelancerId) => FreelancerId = freelancerId;
}
