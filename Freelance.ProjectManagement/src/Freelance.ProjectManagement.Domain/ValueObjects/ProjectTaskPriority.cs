using System.Diagnostics.CodeAnalysis;
using Shared.Domain.Common;

namespace Freelance.ProjectManagement.Domain.ValueObjects;

public sealed class ProjectTaskPriority : ValueObject
{
    public static readonly ProjectTaskPriority Low = new("Low");
    public static readonly ProjectTaskPriority Medium = new("Medium");
    public static readonly ProjectTaskPriority High = new("High");

    public static IReadOnlyList<ProjectTaskPriority> All { get; } = [Low, Medium, High];

    private ProjectTaskPriority(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static bool IsValid(string? value) => TryFromName(value, out _);

    public static bool TryFromName(string? value, [NotNullWhen(true)] out ProjectTaskPriority? priority)
    {
        priority = All.FirstOrDefault(p => string.Equals(p.Value, value, StringComparison.Ordinal));
        return priority is not null;
    }

    public static ProjectTaskPriority FromName(string value) =>
        TryFromName(value, out var priority)
            ? priority
            : throw new ArgumentOutOfRangeException(nameof(value), value, $"Unknown task priority '{value}'.");

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
