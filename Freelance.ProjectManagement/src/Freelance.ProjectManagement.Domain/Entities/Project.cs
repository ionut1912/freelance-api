using Freelance.ProjectManagement.Domain.Exceptions;
using Freelance.ProjectManagement.Domain.ValueObjects;
using Shared.Domain.Common;

namespace Freelance.ProjectManagement.Domain.Entities;

public class Project : Entity
{
    private Project()
    {
    }

    private Project(string title, string description, DateTime deadline, decimal amount, string currency)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title cannot be null or empty", nameof(title));
        }
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("Description cannot be null or empty", nameof(description));
        }
        Title = title;
        Description = description;
        Deadline = deadline.Kind == DateTimeKind.Utc
        ? deadline
        : DateTime.SpecifyKind(deadline, DateTimeKind.Utc);
        Budget = Money.Create(amount, currency);
    }

    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public DateTime Deadline { get; private set; }
    public Guid ClientId { get; private set; }
    public Guid FreelancerId { get; private set; }
    private readonly List<ProjectTechnology> _technologies = [];
    public IReadOnlyCollection<ProjectTechnology> Technologies => _technologies.AsReadOnly();
    private readonly List<ProjectTask> _tasks = [];
    public IReadOnlyCollection<ProjectTask> Tasks => _tasks.AsReadOnly();
    public Money Budget { get; private set; } = Money.Create(0, "USD");

    public static Project Create(string title, string description, DateTime deadline, decimal amount, string currency)
    {
        return new Project(title, description, deadline, amount, currency);
    }

    public void AddTechnologies(IEnumerable<ProjectTechnology> technologies)
    {
        foreach (var technology in technologies)
        {
            if (!_technologies.Any(t => t.Technology == technology.Technology))
            {
                _technologies.Add(technology);
            }
        }
    }

    public void AddTask(ProjectTask task)
    {
        ArgumentNullException.ThrowIfNull(task);
        _tasks.Add(task);
    }

    public void Update(string title, string description, DateTime deadline, decimal amount, string currency)
    {
        if (!string.IsNullOrEmpty(title))
        {
            Title = title;
        }
        if (!string.IsNullOrEmpty(description))
        {
            Description = description;
        }
        Deadline = deadline.Kind == DateTimeKind.Utc
        ? deadline
        : DateTime.SpecifyKind(deadline, DateTimeKind.Utc);
        Budget = Money.Create(amount, currency) ?? Budget;
    }

    public void UpdateTask(ProjectTask task)
    {
        var exitingTask = _tasks.FirstOrDefault(x => x.Id == task.Id);
        if (exitingTask == null)
        {
            throw new ProjectTaskNotFoundException($"Project task with id {task.Id} was not found");
        }
        exitingTask.Update(task.Title, task.Description, task.Status, task.Priority);
    }

    public void UpdateTechnologies(IEnumerable<ProjectTechnology> technologies)
    {
        foreach (var technology in technologies)
        {
            if (!_technologies.Any(t => t.Technology == technology.Technology))
            {
                _technologies.Add(technology);
            }
        }
    }

    public void AssignClient(Guid clientId)
    {
        ClientId = clientId;
    }

    public void AssignFreelancer(Guid freelancerId)
    {
        FreelancerId = freelancerId;
    }
}