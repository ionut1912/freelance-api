using Shared.Domain.Common;

namespace Freelance.ProjectManagement.Domain.ValueObjects;

public class ProjectTaskPriority : ValueObject
{
    public static readonly ProjectTaskPriority Low = new("Low");
    public static readonly ProjectTaskPriority Medium = new("Medium");
    public static readonly ProjectTaskPriority High = new("High");

    private ProjectTaskPriority(string value)
    {
        Value = value;
    }

    public string Value { get; }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
