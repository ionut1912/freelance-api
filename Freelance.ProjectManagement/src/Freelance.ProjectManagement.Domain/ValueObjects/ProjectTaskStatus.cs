using Shared.Domain.Common;

namespace Freelance.ProjectManagement.Domain.ValueObjects;

public class ProjectTaskStatus : ValueObject
{
    public static readonly ProjectTaskStatus New = new("New");
    public static readonly ProjectTaskStatus InProgress = new("InProgress");
    public static readonly ProjectTaskStatus Review = new("Review");
    public static readonly ProjectTaskStatus Done = new("Done");

    private ProjectTaskStatus(string value)
    {
        Value = value;
    }

    public string Value { get; }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

}
